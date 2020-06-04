#import <Foundation/Foundation.h>
#import <Photos/Photos.h>
#import <MobileCoreServices/UTCoreTypes.h>
#import <ImageIO/ImageIO.h>
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
#import <AssetsLibrary/AssetsLibrary.h>
#endif
#import<SystemConfiguration/CaptiveNetwork.h>

#import <AVFoundation/AVFoundation.h>

#import "WAVideoBox.h"

#ifdef UNITY_4_0 || UNITY_5_0
#import "iPhone_View.h"
#else
extern UIViewController* UnityGetGLViewController();
#endif

@interface UNativeGallery:NSObject
+ (int)checkPermission; // 相册
+ (int)requestPermission;

+ (int)checkMicrophonePermission; // 麦克风
+ (int)requestMicrophonePermission;

+ (int)checkLocationPermission; // 位置
+ (int)requestLocationPermission;


+ (int)checkCameraPermission; // 相机
+ (int)requestCameraPermission;

+ (int)canOpenSettings;
+ (void)openSettings;

+ (void)buildVersionStr;
+ (void)buildStr;

+ (void)saveMedia:(NSString *)path albumName:(NSString *)album isImg:(BOOL)isImg;
+ (void)pickMedia:(BOOL)imageMode savePath:(NSString *)mediaSavePath;
+ (void)pickPhotoFromCamera:(BOOL)imageMode savePath:(NSString *)mediaSavePath;

+ (int)isMediaPickerBusy;

+ (char *)getSSIDProperties:(NSString *)path;

+ (char *)getImageProperties:(NSString *)path;
+ (char *)getVideoProperties:(NSString *)path;
+ (char *)loadImageAtPath:(NSString *)path tempFilePath:(NSString *)tempFilePath maximumSize:(int)maximumSize;
@end

@implementation UNativeGallery

static NSString *pickedMediaSavePath;
static NSString *resultPath;
static NSString *resultSSIDStr;
static NSString *resultBuildVersionStr;
static UIPopoverController *popup;
static UIImagePickerController *imagePicker;
static CLLocationManager *locationManager;
static int imagePickerState = 0; // 0 -> none, 1 -> showing (always in this state on iPad), 2 -> finished
static int locationState = 0; // 0 -> wait , 1 -> yes, 2 -> no
static WAVideoBox *videoBox;

+ (char *)getSSIDProperties:(NSString *)path{
    NSLog(@"getSSIDProperties - %s",__func__);
    char *empty = nullptr;
    return empty;
}

+ (void)buildVersionStr
{
    NSString *buildVStr = [NSString stringWithFormat:@"%@%@ #build %@",OB_APP_NAME,OB_APP_VERSION,OB_APP_BUILD];
    UnitySendMessage("NGMediaReceiveCallbackiOS", "GetBuildVersionResult", [buildVStr UTF8String]);
}

+ (void)buildStr
{
    UnitySendMessage("NGMediaReceiveCallbackiOS", "GetBuildVersionResult", [OB_APP_BUILD UTF8String]);
}


// 相机
+ (int)checkCameraPermission{
    AVAuthorizationStatus permission = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
    if (permission == AVAuthorizationStatusAuthorized)
    {
        return 1;
        /* code */
    }
    else if (permission == AVAuthorizationStatusNotDetermined)
    {
        return 2;
        /* code */
    }
    else
        return 0;

}
+ (int)requestCameraPermission
{
    AVAuthorizationStatus permission = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
    if (permission == AVAuthorizationStatusAuthorized)
    {
        return 1;
        /* code */
    }
    else if (permission == AVAuthorizationStatusNotDetermined)
    {
        __block BOOL authorized = NO;
        [AVCaptureDevice requestAccessForMediaType:AVMediaTypeVideo completionHandler:^(BOOL granted) {
            if (granted)
                authorized = YES;
            else
                authorized = NO;

        }];
        return authorized;
        /* code */
    }
    else
        return 0;
}


///MARK: 获取wifi名
+ (void)getWifiSSID{
    NSString *wifiName = nil;
    CFArrayRef wifiInterfaces = CNCopySupportedInterfaces();
    if (!wifiInterfaces) {
        resultSSIDStr = nil;
        UnitySendMessage("NGMediaReceiveCallbackiOS", "GetSSIDResult", "");
    }
    NSArray *interfaces = (__bridge NSArray *)wifiInterfaces;
    for (NSString *interfaceName in interfaces) {
        CFDictionaryRef dictRef = CNCopyCurrentNetworkInfo((__bridge CFStringRef)(interfaceName));
        
        if (dictRef) {
            NSDictionary *networkInfo = (__bridge NSDictionary *)dictRef;
            NSLog(@"network info -> %@", networkInfo);
            wifiName = [networkInfo objectForKey:(__bridge NSString *)kCNNetworkInfoKeySSID];
            CFRelease(dictRef);
        }
    }
    resultSSIDStr = wifiName;
    CFRelease(wifiInterfaces);
    if (wifiName && wifiName.length > 0) {
        UnitySendMessage("NGMediaReceiveCallbackiOS", "GetSSIDResult", (char *)[wifiName UTF8String]);
    }
}



// 位置
+ (int)checkLocationPermission{
    CLAuthorizationStatus authorizationStatus = [CLLocationManager authorizationStatus];
    if (authorizationStatus == kCLAuthorizationStatusAuthorizedAlways || authorizationStatus == kCLAuthorizationStatusAuthorizedWhenInUse)
        return 1;
    else if (authorizationStatus == kCLAuthorizationStatusDenied)
        return 0;
    else
        return 2;
}
+ (int)requestLocationPermission{
    CLAuthorizationStatus authorizationStatus = [CLLocationManager authorizationStatus];
    if (authorizationStatus == kCLAuthorizationStatusAuthorizedAlways || authorizationStatus == kCLAuthorizationStatusAuthorizedWhenInUse)
        return 1;
    else if (authorizationStatus == kCLAuthorizationStatusDenied)
        return 0;
    else
        {
            if (![CLLocationManager locationServicesEnabled]) {
                return 2;
            }
            locationManager = [[CLLocationManager alloc] init];
            locationManager.delegate = self;
            locationManager.desiredAccuracy = kCLLocationAccuracyBestForNavigation;
            
            if ([CLLocationManager authorizationStatus] == kCLAuthorizationStatusNotDetermined) {
                BOOL hasAlwaysKey = [[NSBundle mainBundle]
                                     objectForInfoDictionaryKey:@"NSLocationAlwaysUsageDescription"] != nil;
                BOOL hasWhenInUseKey =
                [[NSBundle mainBundle] objectForInfoDictionaryKey:@"NSLocationWhenInUseUsageDescription"] !=
                nil;
                if (hasAlwaysKey) {
                    [locationManager requestAlwaysAuthorization];
                } else if (hasWhenInUseKey) {
                    [locationManager requestWhenInUseAuthorization];
                } else {
                    // At least one of the keys NSLocationAlwaysUsageDescription or
                    // NSLocationWhenInUseUsageDescription MUST be present in the Info.plist
                    // file to use location services on iOS 8+.
                    NSAssert(hasAlwaysKey || hasWhenInUseKey,
                             @"To use location services in iOS 8+, your Info.plist must "
                             @"provide a value for either "
                             @"NSLocationWhenInUseUsageDescription or "
                             @"NSLocationAlwaysUsageDescription.");
                }
            }
            [locationManager startUpdatingLocation];
            return 2;
        }
}

// 麦克风
+ (int)checkMicrophonePermission{
    AVAudioSessionRecordPermission permission = [[AVAudioSession sharedInstance] recordPermission];
    if (permission == AVAudioSessionRecordPermissionGranted)
        return 1;
    else if (permission == AVAudioSessionRecordPermissionDenied)
        return 2;
    else
        return 0;
}

+ (int)requestMicrophonePermission{
    AVAudioSessionRecordPermission status = [[AVAudioSession sharedInstance] recordPermission];
    if (status == AVAudioSessionRecordPermissionGranted) {
        return 1;
    }
    else if (status == AVAudioSessionRecordPermissionUndetermined) {
        __block BOOL authorized = NO;
        dispatch_semaphore_t sema = dispatch_semaphore_create(0);
        [[AVAudioSession sharedInstance] requestRecordPermission:^(BOOL granted) {
            authorized = granted;
            dispatch_semaphore_signal(sema);
        }];
        dispatch_semaphore_wait(sema, DISPATCH_TIME_FOREVER);
        if (authorized)
            return 1;
        else
            return 0;
    }
    else {
        return 0;
    }
}

#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
+ (int)checkPermission {
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
	if ([[[UIDevice currentDevice] systemVersion] compare:@"8.0" options:NSNumericSearch] != NSOrderedAscending)
	{
#endif
		// version >= iOS 8: check permission using Photos framework
		PHAuthorizationStatus status = [PHPhotoLibrary authorizationStatus];
		if (status == PHAuthorizationStatusAuthorized)
			return 1;
		else if (status == PHAuthorizationStatusNotDetermined )
			return 2;
		else
			return 0;
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
	}
	else
	{
		// version < iOS 8: check permission using AssetsLibrary framework (Photos framework not available)
		ALAuthorizationStatus status = [ALAssetsLibrary authorizationStatus];
		if (status == ALAuthorizationStatusAuthorized)
			return 1;
		else if (status == ALAuthorizationStatusNotDetermined)
			return 2;
		else
			return 0;
	}
#endif
}
#pragma clang diagnostic pop

+ (int)requestPermission {
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
	if ([[[UIDevice currentDevice] systemVersion] compare:@"8.0" options:NSNumericSearch] != NSOrderedAscending)
	{
#endif
		// version >= iOS 8: request permission using Photos framework
		return [self requestPermissionNew];
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
	}
	else
	{
		// version < iOS 8: request permission using AssetsLibrary framework (Photos framework not available)
		return [self requestPermissionOld];
	}
#endif
}

// Credit: https://stackoverflow.com/a/25453667/2373034
+ (int)canOpenSettings {
	if (&UIApplicationOpenSettingsURLString != NULL)
		return 1;
	else
		return 0;
}

// Credit: https://stackoverflow.com/a/25453667/2373034
+ (void)openSettings {
	if (&UIApplicationOpenSettingsURLString != NULL)
		[[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
}

+ (void)saveMedia:(NSString *)path albumName:(NSString *)album isImg:(BOOL)isImg {
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
	if ([[[UIDevice currentDevice] systemVersion] compare:@"8.0" options:NSNumericSearch] != NSOrderedAscending)
	{
#endif
		// version >= iOS 8: save to specified album using Photos framework
		[self saveMediaNew:path albumName:album isImage:isImg];
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
	}
	else
	{
		// version < iOS 8: save using AssetsLibrary framework (Photos framework not available)
		[self saveMediaOld:path albumName:album isImage:isImg];
	}
#endif
}

// Credit: https://stackoverflow.com/a/26933380/2373034
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
+ (int)requestPermissionOld {
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
	ALAuthorizationStatus status = [ALAssetsLibrary authorizationStatus];
	
	if (status == ALAuthorizationStatusAuthorized) {
		return 1;
	}
	else if (status == ALAuthorizationStatusNotDetermined) {
		__block BOOL authorized = NO;
		ALAssetsLibrary *lib = [[ALAssetsLibrary alloc] init];
		
		dispatch_semaphore_t sema = dispatch_semaphore_create(0);
		[lib enumerateGroupsWithTypes:ALAssetsGroupAll usingBlock:^(ALAssetsGroup *group, BOOL *stop) {
			*stop = YES;
			authorized = YES;
			dispatch_semaphore_signal(sema);
		} failureBlock:^(NSError *error) {
			dispatch_semaphore_signal(sema);
		}];
		dispatch_semaphore_wait(sema, DISPATCH_TIME_FOREVER);
		
		if (authorized)
			return 1;
		else
			return 0;
	}
	else {
		return 0;
	}
#endif
	
	return 0;
}
#pragma clang diagnostic pop

// Credit: https://stackoverflow.com/a/32989022/2373034
+ (int)requestPermissionNew {
	PHAuthorizationStatus status = [PHPhotoLibrary authorizationStatus];
	
	if (status == PHAuthorizationStatusAuthorized) {
		return 1;
	}
	else if (status == PHAuthorizationStatusNotDetermined) {
		__block BOOL authorized = NO;
		
		dispatch_semaphore_t sema = dispatch_semaphore_create(0);
		[PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
			authorized = (status == PHAuthorizationStatusAuthorized);
			dispatch_semaphore_signal(sema);
		}];
		dispatch_semaphore_wait(sema, DISPATCH_TIME_FOREVER);
		
		if (authorized)
			return 1;
		else
			return 0;
	}
	else {
		return 0;
	}
}

// Credit: https://stackoverflow.com/a/22056664/2373034
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
+ (void)saveMediaOld:(NSString *)path albumName:(NSString *)album isImage:(BOOL)isImage {
#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
	ALAssetsLibrary *library = [[ALAssetsLibrary alloc] init];
	
	if (!isImage && ![library videoAtPathIsCompatibleWithSavedPhotosAlbum:[NSURL fileURLWithPath:path]])
	{
		[[NSFileManager defaultManager] removeItemAtPath:path error:nil];
		UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveFailed", "Video format is not compatible with Photos");
		return;
	}
	
	void (^saveBlock)(ALAssetsGroup *assetCollection) = ^void(ALAssetsGroup *assetCollection) {
		void (^saveResultBlock)(NSURL *assetURL, NSError *error) = ^void(NSURL *assetURL, NSError *error) {
			[[NSFileManager defaultManager] removeItemAtPath:path error:nil];
			
			if (error.code == 0) {
				[library assetForURL:assetURL resultBlock:^(ALAsset *asset) {
					[assetCollection addAsset:asset];
					UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveCompleted", "");
				} failureBlock:^(NSError* error) {
					NSLog(@"Error moving asset to album: %@", error);
					UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveFailed", [self getCString:[error localizedDescription]]);
				}];
			}
			else {
				NSLog(@"Error creating asset: %@", error);
				UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveFailed", [self getCString:[error localizedDescription]]);
			}
		};
		
		if (!isImage)
			[library writeImageDataToSavedPhotosAlbum:[NSData dataWithContentsOfFile:path] metadata:nil completionBlock:saveResultBlock];
		else
			[library writeVideoAtPathToSavedPhotosAlbum:[NSURL fileURLWithPath:path] completionBlock:saveResultBlock];
	};
	
	__block BOOL albumFound = NO;
	[library enumerateGroupsWithTypes:ALAssetsGroupAlbum usingBlock:^(ALAssetsGroup *group, BOOL *stop) {
		if ([[group valueForProperty:ALAssetsGroupPropertyName] isEqualToString:album]) {
			*stop = YES;
			albumFound = YES;
			saveBlock(group);
		}
		else if (group == nil && albumFound==NO) { // Album doesn't exist
			[library addAssetsGroupAlbumWithName:album resultBlock:^(ALAssetsGroup *group) {
				saveBlock(group);
			}
									failureBlock:^(NSError *error) {
										NSLog(@"Error creating album: %@", error);
										[[NSFileManager defaultManager] removeItemAtPath:path error:nil];
										UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveFailed", [self getCString:[error localizedDescription]]);
									}];
		}
	} failureBlock:^(NSError* error) {
		NSLog(@"Error listing albums: %@", error);
		[[NSFileManager defaultManager] removeItemAtPath:path error:nil];
		UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveFailed", [self getCString:[error localizedDescription]]);
	}];
#endif
}
#pragma clang diagnostic pop

// Credit: https://stackoverflow.com/a/39909129/2373034
+ (void)saveMediaNew:(NSString *)path albumName:(NSString *)album isImage:(BOOL)isImage {
	void (^saveBlock)(PHAssetCollection *assetCollection) = ^void(PHAssetCollection *assetCollection) {
		[[PHPhotoLibrary sharedPhotoLibrary] performChanges:^{
			PHAssetChangeRequest *assetChangeRequest;
			if (isImage)
				assetChangeRequest = [PHAssetChangeRequest creationRequestForAssetFromImageAtFileURL:[NSURL fileURLWithPath:path]];
			else
				assetChangeRequest = [PHAssetChangeRequest creationRequestForAssetFromVideoAtFileURL:[NSURL fileURLWithPath:path]];
			
			PHAssetCollectionChangeRequest *assetCollectionChangeRequest = [PHAssetCollectionChangeRequest changeRequestForAssetCollection:assetCollection];
			[assetCollectionChangeRequest addAssets:@[[assetChangeRequest placeholderForCreatedAsset]]];
			
		} completionHandler:^(BOOL success, NSError *error) {
			[[NSFileManager defaultManager] removeItemAtPath:path error:nil];
			
			if (success)
				UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveCompleted", "");
			else {
				NSLog(@"Error creating asset: %@", error);
				UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveFailed", [self getCString:[error localizedDescription]]);
			}
		}];
	};
	
	PHFetchOptions *fetchOptions = [[PHFetchOptions alloc] init];
	fetchOptions.predicate = [NSPredicate predicateWithFormat:@"localizedTitle = %@", album];
	PHFetchResult *fetchResult = [PHAssetCollection fetchAssetCollectionsWithType:PHAssetCollectionTypeAlbum subtype:PHAssetCollectionSubtypeAny options:fetchOptions];
	if (fetchResult.count > 0) {
		saveBlock(fetchResult.firstObject);
	}
	else {
		__block PHObjectPlaceholder *albumPlaceholder;
		[[PHPhotoLibrary sharedPhotoLibrary] performChanges:^{
			PHAssetCollectionChangeRequest *changeRequest = [PHAssetCollectionChangeRequest creationRequestForAssetCollectionWithTitle:album];
			albumPlaceholder = changeRequest.placeholderForCreatedAssetCollection;
		} completionHandler:^(BOOL success, NSError *error) {
			if (success) {
				PHFetchResult *fetchResult = [PHAssetCollection fetchAssetCollectionsWithLocalIdentifiers:@[albumPlaceholder.localIdentifier] options:nil];
				if (fetchResult.count > 0)
					saveBlock(fetchResult.firstObject);
				else {
					[[NSFileManager defaultManager] removeItemAtPath:path error:nil];
					UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveFailed", "Album placeholder not found" );
				}
			}
			else {
				NSLog(@"Error creating album: %@", error);
				[[NSFileManager defaultManager] removeItemAtPath:path error:nil];
				UnitySendMessage("NGMediaSaveCallbackiOS", "OnMediaSaveFailed", [self getCString:[error localizedDescription]]);
			}
		}];
	}
}


// Credit: https://stackoverflow.com/a/10531752/2373034
+ (void)pickPhotoFromCamera:(BOOL)imageMode savePath:(NSString *)mediaSavePath {
	imagePicker = [[UIImagePickerController alloc] init];
	imagePicker.delegate = self;
	imagePicker.allowsEditing = YES;
	UIImagePickerControllerSourceType sourceType = UIImagePickerControllerSourceTypeCamera;
	if (![UIImagePickerController isSourceTypeAvailable: UIImagePickerControllerSourceTypeCamera]) {
       sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    }
	imagePicker.sourceType = sourceType;
	
	if (imageMode)
		imagePicker.mediaTypes = [NSArray arrayWithObject:(NSString *)kUTTypeImage];
	else
	{
		imagePicker.mediaTypes = [NSArray arrayWithObjects:(NSString *)kUTTypeMovie, (NSString *)kUTTypeVideo, nil];
        imagePicker.videoMaximumDuration = 60.f;
        imagePicker.videoQuality = UIImagePickerControllerQualityTypeIFrame960x540;
        UnitySendMessage("App", "IPhonePlayerVideoCompress", "");
#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 110000
		// Don't compress the picked video if possible
		if ([[[UIDevice currentDevice] systemVersion] compare:@"11.0" options:NSNumericSearch] != NSOrderedAscending)
			imagePicker.videoExportPreset = AVAssetExportPresetPassthrough;
#endif
	}
	
	pickedMediaSavePath = mediaSavePath;
	
	imagePickerState = 1;
	UIViewController *rootViewController = UnityGetGLViewController();
	if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone) // iPhone
		[rootViewController presentViewController:imagePicker animated:YES completion:^{ imagePickerState = 0; }];
	else { // iPad
		popup = [[UIPopoverController alloc] initWithContentViewController:imagePicker];
		popup.delegate = self;
		[popup presentPopoverFromRect:CGRectMake( rootViewController.view.frame.size.width / 2, rootViewController.view.frame.size.height / 2, 1, 1 ) inView:rootViewController.view permittedArrowDirections:0 animated:YES];
	}
}

// Credit: https://stackoverflow.com/a/10531752/2373034
+ (void)pickMedia:(BOOL)imageMode savePath:(NSString *)mediaSavePath {
	imagePicker = [[UIImagePickerController alloc] init];
	imagePicker.delegate = self;
	imagePicker.allowsEditing = YES;
	imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
	
	if (imageMode)
		imagePicker.mediaTypes = [NSArray arrayWithObject:(NSString *)kUTTypeImage];
	else
	{
		imagePicker.mediaTypes = [NSArray arrayWithObjects:(NSString *)kUTTypeMovie, (NSString *)kUTTypeVideo, nil];
		
#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 110000
		// Don't compress the picked video if possible
		if ([[[UIDevice currentDevice] systemVersion] compare:@"11.0" options:NSNumericSearch] != NSOrderedAscending)
			imagePicker.videoExportPreset = AVAssetExportPresetPassthrough;
#endif
	}
	
	pickedMediaSavePath = mediaSavePath;
	
	imagePickerState = 1;
	UIViewController *rootViewController = UnityGetGLViewController();
	if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone) // iPhone
		[rootViewController presentViewController:imagePicker animated:YES completion:^{ imagePickerState = 0; }];
	else { // iPad
		popup = [[UIPopoverController alloc] initWithContentViewController:imagePicker];
		popup.delegate = self;
		[popup presentPopoverFromRect:CGRectMake( rootViewController.view.frame.size.width / 2, rootViewController.view.frame.size.height / 2, 1, 1 ) inView:rootViewController.view permittedArrowDirections:0 animated:YES];
	}
}

+ (int)isMediaPickerBusy {
	if (imagePickerState == 2)
		return 1;
	
	if (imagePicker != nil) {
		if (imagePickerState == 1 || [imagePicker presentingViewController] == UnityGetGLViewController())
			return 1;
		else {
			imagePicker = nil;
			return 0;
		}
	}
	else
		return 0;
}

// Credit: https://stackoverflow.com/a/4170099/2373034
+ (NSArray *)getImageMetadata:(NSString *)path {
	int width = 0;
	int height = 0;
	int orientation = -1;
	
	CGImageSourceRef imageSource = CGImageSourceCreateWithURL((__bridge CFURLRef)[NSURL fileURLWithPath:path], nil);
	if (imageSource != nil) {
		NSDictionary *options = [NSDictionary dictionaryWithObject:[NSNumber numberWithBool:NO] forKey:(__bridge NSString *)kCGImageSourceShouldCache];
		CFDictionaryRef imageProperties = CGImageSourceCopyPropertiesAtIndex(imageSource, 0, (__bridge CFDictionaryRef)options);
		CFRelease(imageSource);
		
		CGFloat widthF = 0.0f, heightF = 0.0f;
		if (imageProperties != nil) {
			if (CFDictionaryContainsKey(imageProperties, kCGImagePropertyPixelWidth))
				CFNumberGetValue((CFNumberRef)CFDictionaryGetValue(imageProperties, kCGImagePropertyPixelWidth), kCFNumberCGFloatType, &widthF);
			
			if (CFDictionaryContainsKey(imageProperties, kCGImagePropertyPixelHeight))
				CFNumberGetValue((CFNumberRef)CFDictionaryGetValue(imageProperties, kCGImagePropertyPixelHeight), kCFNumberCGFloatType, &heightF);
			
			if (CFDictionaryContainsKey(imageProperties, kCGImagePropertyOrientation)) {
				CFNumberGetValue((CFNumberRef)CFDictionaryGetValue(imageProperties, kCGImagePropertyOrientation), kCFNumberIntType, &orientation);
				
				if (orientation > 4) { // landscape image
					CGFloat temp = widthF;
					widthF = heightF;
					heightF = temp;
				}
			}
			
			CFRelease(imageProperties);
		}
		
		width = (int)roundf(widthF);
		height = (int)roundf(heightF);
	}
	
	return [[NSArray alloc] initWithObjects:[NSNumber numberWithInt:width], [NSNumber numberWithInt:height], [NSNumber numberWithInt:orientation], nil];
}

+ (char *)getImageProperties:(NSString *)path {
	NSArray *metadata = [self getImageMetadata:path];
	
	int orientationUnity;
	int orientation = [metadata[2] intValue];
	
	// To understand the magic numbers, see ImageOrientation enum in NativeGallery.cs
	// and http://sylvana.net/jpegcrop/exif_orientation.html
	if (orientation == 1)
		orientationUnity = 0;
	else if (orientation == 2)
		orientationUnity = 4;
	else if (orientation == 3)
		orientationUnity = 2;
	else if (orientation == 4)
		orientationUnity = 6;
	else if (orientation == 5)
		orientationUnity = 5;
	else if (orientation == 6)
		orientationUnity = 1;
	else if (orientation == 7)
		orientationUnity = 7;
	else if (orientation == 8)
		orientationUnity = 3;
	else
		orientationUnity = -1;
	
	return [self getCString:[NSString stringWithFormat:@"%d>%d> >%d", [metadata[0] intValue], [metadata[1] intValue], orientationUnity]];
}

+ (char *)getVideoProperties:(NSString *)path {
	CGSize size = CGSizeZero;
	float rotation = 0;
	long long duration = 0;
	
	AVURLAsset *asset = [AVURLAsset URLAssetWithURL:[NSURL fileURLWithPath:path] options:nil];
	if (asset != nil) {
		duration = (long long) round(CMTimeGetSeconds([asset duration]) * 1000);
		CGAffineTransform transform = [asset preferredTransform];
		NSArray<AVAssetTrack *>* videoTracks = [asset tracksWithMediaType:AVMediaTypeVideo];
		if (videoTracks != nil && [videoTracks count] > 0) {
			size = [[videoTracks objectAtIndex:0] naturalSize];
			transform = [[videoTracks objectAtIndex:0] preferredTransform];
		}
		
		rotation = atan2(transform.b, transform.a) * (180.0 / M_PI);
	}
	
	return [self getCString:[NSString stringWithFormat:@"%d>%d>%lld>%f", (int)roundf(size.width), (int)roundf(size.height), duration, rotation]];
}

+ (UIImage *)scaleImage:(UIImage *)image maxSize:(int)maxSize {
	CGFloat width = image.size.width;
	CGFloat height = image.size.height;
	
	UIImageOrientation orientation = image.imageOrientation;
	if (width <= maxSize && height <= maxSize && orientation != UIImageOrientationDown &&
		orientation != UIImageOrientationLeft && orientation != UIImageOrientationRight &&
		orientation != UIImageOrientationLeftMirrored && orientation != UIImageOrientationRightMirrored &&
		orientation != UIImageOrientationUpMirrored && orientation != UIImageOrientationDownMirrored)
		return image;
	
	CGFloat scaleX = 1.0f;
	CGFloat scaleY = 1.0f;
	if (width > maxSize)
		scaleX = maxSize / width;
	if (height > maxSize)
		scaleY = maxSize / height;
	
	// Credit: https://github.com/mbcharbonneau/UIImage-Categories/blob/master/UIImage%2BAlpha.m
	CGImageAlphaInfo alpha = CGImageGetAlphaInfo(image.CGImage);
	BOOL hasAlpha = alpha == kCGImageAlphaFirst || alpha == kCGImageAlphaLast || alpha == kCGImageAlphaPremultipliedFirst || alpha == kCGImageAlphaPremultipliedLast;
	
	CGFloat scaleRatio = scaleX < scaleY ? scaleX : scaleY;
	CGRect imageRect = CGRectMake(0, 0, width * scaleRatio, height * scaleRatio);
	UIGraphicsBeginImageContextWithOptions(imageRect.size, !hasAlpha, image.scale);
	[image drawInRect:imageRect];
	image = UIGraphicsGetImageFromCurrentImageContext();
	UIGraphicsEndImageContext();
	
	return image;
}

+ (char *)loadImageAtPath:(NSString *)path tempFilePath:(NSString *)tempFilePath maximumSize:(int)maximumSize {
	// Check if the image can be loaded by Unity without requiring a conversion to PNG
	// Credit: https://stackoverflow.com/a/12048937/2373034
	NSString *extension = [path pathExtension];
	BOOL conversionNeeded = [extension caseInsensitiveCompare:@"jpg"] != NSOrderedSame && [extension caseInsensitiveCompare:@"jpeg"] != NSOrderedSame && [extension caseInsensitiveCompare:@"png"] != NSOrderedSame;

	if (!conversionNeeded) {
		// Check if the image needs to be processed at all
		NSArray *metadata = [self getImageMetadata:path];
		int orientationInt = [metadata[2] intValue];  // 1: correct orientation, [1,8]: valid orientation range
		if (orientationInt == 1 && [metadata[0] intValue] <= maximumSize && [metadata[1] intValue] <= maximumSize)
			return [self getCString:path];
	}
	
	UIImage *image = [UIImage imageWithContentsOfFile:path];
	if (image == nil)
		return [self getCString:path];
	
	UIImage *scaledImage = [self scaleImage:image maxSize:maximumSize];
	if (conversionNeeded || scaledImage != image) {
		if (![UIImagePNGRepresentation(scaledImage) writeToFile:tempFilePath atomically:YES]) {
			NSLog(@"Error creating scaled image");
			return [self getCString:path];
		}
		
		return [self getCString:tempFilePath];
	}
	else
		return [self getCString:path];
}


-(void)locationManager:(CLLocationManager *)manager didChangeAuthorizationStatus:(CLAuthorizationStatus)status
{
    if (status == kCLAuthorizationStatusNotDetermined) {
        
    }else if (status == kCLAuthorizationStatusAuthorizedWhenInUse || status == kCLAuthorizationStatusAuthorizedAlways){
        [locationManager stopUpdatingLocation];
        locationManager = nil;
        locationState = 1;
        [UNativeGallery getWifiSSID];
    }else if (status == kCLAuthorizationStatusDenied || status == kCLAuthorizationStatusRestricted){
        [locationManager stopUpdatingLocation];
        locationManager = nil;
        locationState = 2;
        [UNativeGallery getWifiSSID];
    }
}

#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
+ (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info {
	resultPath = nil;
	NSLog(@"imagePickerController :%@",info);
    
	if ([info[UIImagePickerControllerMediaType] isEqualToString:(NSString *)kUTTypeImage]) { // image picked
        UIImage *editedImage = info[UIImagePickerControllerEditedImage];
        UIImage *compressImage = [self compressImage:editedImage WithMaxLength:1024.f * 20];
        NSData *imageData = UIImageJPEGRepresentation(compressImage, 1);
        NSError *error;
        resultPath = [[NSSearchPathForDirectoriesInDomains(NSCachesDirectory, NSUserDomainMask, YES) objectAtIndex:0] stringByAppendingString:@"/pickedImg.png"];

        if (![[NSFileManager defaultManager] fileExistsAtPath:resultPath] || [[NSFileManager defaultManager] removeItemAtPath:resultPath error:&error]) {
            if (![imageData writeToFile:resultPath atomically:YES]) {
                NSLog(@"Error copying source image to file");
                resultPath = nil;
            }
        }
        
        popup = nil;
        imagePicker = nil;
        imagePickerState = 2;
        UnitySendMessage("NGMediaReceiveCallbackiOS", "OnMediaReceived", [self getCString:resultPath]);
        
        [picker dismissViewControllerAnimated:NO completion:nil];
        return;
	}
	else { // video picked
		NSURL *mediaUrl = info[UIImagePickerControllerMediaURL] ?: info[UIImagePickerControllerReferenceURL];
        NSLog(@"视频的路径:%@,%@",mediaUrl.path,mediaUrl);
        [DRUserManager sharedInstance].lastTimeVideoUrlPath = [mediaUrl path];
        
		if (mediaUrl != nil) {
			resultPath = [mediaUrl path];
            UIImage *videoThumbnail = [self imageWithVideoURL:mediaUrl];
            
            //Crop the image to a square
            CGSize imageSize = videoThumbnail.size;
            CGFloat width = imageSize.width;
            CGFloat height = imageSize.height;
            
            CGFloat newDimension = MIN(width, height);
            if ((width != 300) && (width != height)) {
                CGFloat widthOffset = (width - newDimension) / 2; //x
                CGFloat heightOffset = (height - newDimension) / 2;//y
                UIGraphicsBeginImageContextWithOptions(CGSizeMake(newDimension, newDimension), NO, 0.);
                [videoThumbnail drawAtPoint:CGPointMake(-widthOffset, -heightOffset)
                               blendMode:kCGBlendModeCopy
                                   alpha:1.];
                videoThumbnail = UIGraphicsGetImageFromCurrentImageContext();
                UIGraphicsEndImageContext();
            }
            
            NSString *filePath = [[NSSearchPathForDirectoriesInDomains(NSCachesDirectory, NSUserDomainMask, YES) objectAtIndex:0] stringByAppendingString:@"/videoThumbnail.png"];
            NSError *error;
            if (![[NSFileManager defaultManager] fileExistsAtPath:filePath] || [[NSFileManager defaultManager] removeItemAtPath:filePath error:&error]) {
                if (![UIImageJPEGRepresentation(videoThumbnail, 1) writeToFile:filePath atomically:YES]) {
                    NSLog(@"Error copying source image to file");
                    
                }
            }
            
            videoBox = [WAVideoBox new];
            [videoBox clean];
            videoBox.ratio = WAVideoExportRatio960x540;
            videoBox.videoQuality = 6;
            AVAsset *asset = [AVAsset assetWithURL:mediaUrl];
            Float64 seconds = CMTimeGetSeconds(asset.duration);
            if (seconds < 3) {
                
            }
            [videoBox appendVideoByAsset:[AVAsset assetWithURL:mediaUrl]];
            NSString *videoFilePath = [UNativeGallery buildFilePath];
            
            resultPath = videoFilePath;

//            dispatch_semaphore_t sema = dispatch_semaphore_create(0);

            static NSTimeInterval previousTapTime = 0.0; // Or an ivar
            previousTapTime = CACurrentMediaTime();
            [videoBox asyncFinishEditByFilePath:videoFilePath progress:^(float progress) {
                NSLog(@"视频压缩的进度[%f]",progress);
            } complete:^(NSError *error) {
                if (!error) {
                    NSLog(@"压缩后的视频文件路径 [%@]",videoFilePath);
                    NSLog(@"压缩耗时 :%f",(CACurrentMediaTime() - previousTapTime));
/// 需要告诉 unity 是否压缩完成...
                    resultPath = videoFilePath;
                    UnitySendMessage("App", "IPhonePlayerVideoCompress", "true");
//                    dispatch_semaphore_signal(sema);
                }else{
                    [DRMBProgressHUDTool showSuccess:error.description toView:nil];
                    UnitySendMessage("App", "IPhonePlayerVideoCompress", "false");
                }
            }];
//            dispatch_semaphore_wait(sema, DISPATCH_TIME_FOREVER);
            videoBox = nil;
			// On iOS 13, picked file becomes unreachable as soon as the UIImagePickerController disappears,
			// in that case, copy the video to a temporary location
//			if ([[[UIDevice currentDevice] systemVersion] compare:@"13.0" options:NSNumericSearch] != NSOrderedAscending) {
//				NSError *error;
//				NSString *newPath = [pickedMediaSavePath stringByAppendingPathExtension:[resultPath pathExtension]];
//
//				if (![[NSFileManager defaultManager] fileExistsAtPath:newPath] || [[NSFileManager defaultManager] removeItemAtPath:newPath error:&error]) {
//					if ([[NSFileManager defaultManager] copyItemAtPath:resultPath toPath:newPath error:&error])
//						resultPath = newPath;
//					else {
//						NSLog(@"Error copying video: %@", error);
//						resultPath = nil;
//					}
//				}
//				else {
//					NSLog(@"Error deleting existing video: %@", error);
//					resultPath = nil;
//				}
//			}
		}
	}
	
	popup = nil;
    imagePicker = nil;
    imagePickerState = 2;
    UnitySendMessage("NGMediaReceiveCallbackiOS", "OnMediaReceived", [self getCString:resultPath]);
    
    
    [picker dismissViewControllerAnimated:NO completion:nil];
}

+ (NSString *)buildFilePath{
    
    return [NSTemporaryDirectory() stringByAppendingString:[NSString stringWithFormat:@"%f.mp4", [[NSDate date] timeIntervalSinceReferenceDate]]];
}
#pragma clang diagnostic pop

+ (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker
{
	popup = nil;
	imagePicker = nil;
	UnitySendMessage("NGMediaReceiveCallbackiOS", "OnMediaReceived", "");
	
	[picker dismissViewControllerAnimated:YES completion:nil];
}

+ (void)popoverControllerDidDismissPopover:(UIPopoverController *)popoverController {
	popup = nil;
	imagePicker = nil;
	UnitySendMessage("NGMediaReceiveCallbackiOS", "OnMediaReceived", "");
}

+ (void)trySaveSourceImage:(NSData *)imageData withInfo:(NSDictionary *)info {
        
	NSString *filePath = info[@"PHImageFileURLKey"];
	if (filePath != nil) // filePath can actually be an NSURL, convert it to NSString
		filePath = [NSString stringWithFormat:@"%@", filePath];
	
	if (filePath == nil || [filePath length] == 0)
	{
		filePath = info[@"PHImageFileUTIKey"];
		if (filePath != nil)
			filePath = [NSString stringWithFormat:@"%@", filePath];
	}
	
	if (filePath == nil || [filePath length] == 0)
		resultPath = pickedMediaSavePath;
	else
		resultPath = [pickedMediaSavePath stringByAppendingPathExtension:[filePath pathExtension]];
	
	NSError *error;
	if (![[NSFileManager defaultManager] fileExistsAtPath:resultPath] || [[NSFileManager defaultManager] removeItemAtPath:resultPath error:&error]) {
		if (![imageData writeToFile:resultPath atomically:YES]) {
			NSLog(@"Error copying source image to file");
			resultPath = nil;
		}
	}
	else {
		NSLog(@"Error deleting existing image: %@", error);
		resultPath = nil;
	}
}

// Credit: https://stackoverflow.com/a/37052118/2373034
+ (char *)getCString:(NSString *)source {
	if (source == nil)
		source = @"";
	
	const char *sourceUTF8 = [source UTF8String];
	char *result = (char*) malloc(strlen(sourceUTF8) + 1);
	strcpy(result, sourceUTF8);
	
	return result;
}


+ (UIImage *)compressImage:(UIImage *)image WithMaxLength:(NSUInteger)maxLength{
    // Compress by quality
    CGFloat compression = 1;
    NSData *data = UIImageJPEGRepresentation(image, compression);
    //NSLog(@"Before compressing quality, image size = %ld KB",data.length/1024);
    if (data.length < maxLength) return [UIImage imageWithData:data];
    
    CGFloat max = 1;
    CGFloat min = 0;
    for (int i = 0; i < 6; ++i) {
        compression = (max + min) / 2;
        data = UIImageJPEGRepresentation(image, compression);
        //NSLog(@"Compression = %.1f", compression);
        //NSLog(@"In compressing quality loop, image size = %ld KB", data.length / 1024);
        if (data.length < maxLength * 0.9) {
            min = compression;
        } else if (data.length > maxLength) {
            max = compression;
        } else {
            break;
        }
    }
    //NSLog(@"After compressing quality, image size = %ld KB", data.length / 1024);
    if (data.length < maxLength) return [UIImage imageWithData:data];
    UIImage *resultImage = [UIImage imageWithData:data];
    // Compress by size
    NSUInteger lastDataLength = 0;
    while (data.length > maxLength && data.length != lastDataLength) {
        lastDataLength = data.length;
        CGFloat ratio = (CGFloat)maxLength / data.length;
        //NSLog(@"Ratio = %.1f", ratio);
        CGSize size = CGSizeMake((NSUInteger)(resultImage.size.width * sqrtf(ratio)),
                                 (NSUInteger)(resultImage.size.height * sqrtf(ratio))); // Use NSUInteger to prevent white blank
        UIGraphicsBeginImageContext(size);
        [resultImage drawInRect:CGRectMake(0, 0, size.width, size.height)];
        resultImage = UIGraphicsGetImageFromCurrentImageContext();
        UIGraphicsEndImageContext();
        data = UIImageJPEGRepresentation(resultImage, compression);
        //NSLog(@"In compressing size loop, image size = %ld KB", data.length / 1024);
    }
    //NSLog(@"After compressing size loop, image size = %ld KB", data.length / 1024);
    return [UIImage imageWithData:data];
}

#pragma mark 获取视频的首帧缩略图
+ (UIImage *)imageWithVideoURL:(NSURL *)url
{
    NSDictionary *opts = [NSDictionary dictionaryWithObject:[NSNumber numberWithBool:NO] forKey:AVURLAssetPreferPreciseDurationAndTimingKey];
    AVURLAsset *urlAsset = [AVURLAsset URLAssetWithURL:url options:opts];
    
    AVAssetImageGenerator *generator = [AVAssetImageGenerator assetImageGeneratorWithAsset:urlAsset];
    generator.appliesPreferredTrackTransform = YES;
    generator.maximumSize = CGSizeMake(600, 450);
    NSError *error = nil;
    CGImageRef img = [generator copyCGImageAtTime:CMTimeMake(0, 10000) actualTime:NULL error:&error];
    UIImage *image = [UIImage imageWithCGImage: img];
    return image;
}

@end

extern "C" int _NativeGallery_RequestLocationPermission() {
	return [UNativeGallery requestPermission];
}

extern "C" int _NativeGallery_CheckPermission() {
	return [UNativeGallery checkPermission];
}

extern "C" int _NativeGallery_RequestPermission() {
	return [UNativeGallery requestPermission];
}

extern "C" int _NativeGallery_CanOpenSettings() {
	return [UNativeGallery canOpenSettings];
}

extern "C" void _NativeGallery_OpenSettings() {
	[UNativeGallery openSettings];
}

extern "C" void _NativeGallery_ImageWriteToAlbum(const char* path, const char* album) {
	[UNativeGallery saveMedia:[NSString stringWithUTF8String:path] albumName:[NSString stringWithUTF8String:album] isImg:YES];
}

extern "C" void _NativeGallery_VideoWriteToAlbum(const char* path, const char* album) {
	[UNativeGallery saveMedia:[NSString stringWithUTF8String:path] albumName:[NSString stringWithUTF8String:album] isImg:NO];
}

extern "C" void _NativeGallery_PickImage(const char* imageSavePath) {
    NSLog(@"\n\n unity-ios插件 接到 unity 给 iOS 传参:%@",[NSString stringWithUTF8String:imageSavePath]);
	[UNativeGallery pickMedia:YES savePath:[NSString stringWithUTF8String:imageSavePath]];
}

extern "C" void _NativeGallery_PickVideo(const char* videoSavePath) {
    NSLog(@"\n\n unity-ios插件 接到 unity 给 iOS 传参:%@",[NSString stringWithUTF8String:videoSavePath]);
	[UNativeGallery pickMedia:NO savePath:[NSString stringWithUTF8String:videoSavePath]];
}

extern "C" int _NativeGallery_IsMediaPickerBusy() {
	return [UNativeGallery isMediaPickerBusy];
}

extern "C" char* _NativeGallery_GetImageProperties(const char* path) {
    NSLog(@"\n\n unity-ios插件 接到 unity 给 iOS 传参:%@",[NSString stringWithUTF8String:path]);
	return [UNativeGallery getImageProperties:[NSString stringWithUTF8String:path]];
}

extern "C" char* _NativeGallery_GetSSIDProperties(const char* path) {
    NSLog(@"\n\n unity-ios插件 接到 unity 给 iOS 传参:%@",[NSString stringWithUTF8String:path]);
    return [UNativeGallery getSSIDProperties:[NSString stringWithUTF8String:path]];
}

extern "C" char* _NativeGallery_GetVideoProperties(const char* path) {
    NSLog(@"\n\n unity-ios插件 接到 unity 给 iOS 传参:%@",[NSString stringWithUTF8String:path]);
	return [UNativeGallery getVideoProperties:[NSString stringWithUTF8String:path]];
}

extern "C" char* _NativeGallery_LoadImageAtPath(const char* path, const char* temporaryFilePath, int maxSize) {
	return [UNativeGallery loadImageAtPath:[NSString stringWithUTF8String:path] tempFilePath:[NSString stringWithUTF8String:temporaryFilePath] maximumSize:maxSize];
}

extern "C" int _NativeGallery_CheckLocation(){
    return [UNativeGallery checkLocationPermission];
}

extern "C" int _NativeGallery_RequestLocation(){
    return [UNativeGallery requestLocationPermission];
}

extern "C" int _NativeGallery_CheckMicrophone(){
    return [UNativeGallery checkMicrophonePermission];
}

extern "C" int _NativeGallery_RequestMicrophone(){
    return [UNativeGallery requestMicrophonePermission];
}

extern "C" int _NativeGallery_CheckCamera(){
    return [UNativeGallery checkCameraPermission];
}

extern "C" int _NativeGallery_RequestCamera(){
    return [UNativeGallery requestCameraPermission];
}

extern "C" void _NativeGallery_GetSSID(){
    return [UNativeGallery getWifiSSID];
}

extern "C" void _NativeGallery_GetVersionBuild(){
    return [UNativeGallery buildVersionStr];
}

extern "C" void _NativeGallery_GetBuild(){
    return [UNativeGallery buildStr];
}
extern "C" int _NativeGallery_IsGetSSIDBusy(){
    return 0;
}

extern "C" int _NativeGallery_IsBuildVersionBusy(){
    return 0;
}

extern "C" void _NativeGallery_PickCameraImage(const char* imageSavePath) {
    NSLog(@"\n\n unity-ios插件 接到 unity 给 iOS 传参:%@",[NSString stringWithUTF8String:imageSavePath]);
	[UNativeGallery pickPhotoFromCamera:YES savePath:[NSString stringWithUTF8String:imageSavePath]];
}
extern "C" void _NativeGallery_PickCameraVideo(const char* imageSavePath) {
    NSLog(@"\n\n unity-ios插件 接到 unity 给 iOS 传参:%@",[NSString stringWithUTF8String:imageSavePath]);
	[UNativeGallery pickPhotoFromCamera:NO savePath:[NSString stringWithUTF8String:imageSavePath]];
}