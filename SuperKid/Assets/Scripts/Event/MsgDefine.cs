
using System;
using System.Collections.Generic;
using SuperKid;
using SuperKid.Entity;
using SuperKid.Utils;

public class CanvasAnimationFinish
{
	
}

public class CanvasCanMove
{
	
}

public class CanvasDotMove
{
	
}


public class ListItemButton
{
}
public class MedalDrawSuccess
{
	
}

public class ConnectDeviceRes
{
	private bool isSuccess;
	public bool GetIsSuccess
	{
		get => isSuccess;
		set => isSuccess = value;
	}

	public ConnectDeviceRes(bool success)
	{
		this.isSuccess = success;
	}

	
}
public class SelectPickDate
{
	private string Date;

	public SelectPickDate(string date)
	{
		this.Date = date;
	}
	

	public string GetDate
	{
		get => Date;
		set => Date = value;
	}
}

public class BabyPhoto
{
	private string Path;

	public BabyPhoto(string path)
	{
		this.Path = path;
	}
	

	public string GetPath
	{
		get => Path;
		set => Path = value;
	}
}

public class UserPhoto
{
	private string Path;

	public UserPhoto(string path)
	{
		this.Path = path;
	}
	

	public string GetPath
	{
		get => Path;
		set => Path = value;
	}
}

public class BindDeviceResult
{
	private BindDeviceModel model;

	public BindDeviceResult(BindDeviceModel model)
	{
		this.model = model;
	}

	public BindDeviceModel Model
	{
		get => model;
		set => model = value;
	}
}

public class ChoosePhotoClick
{
	private NativeAction Action;// 2相册，3拍照
	private ChoosePhotoAction PhotoAction; // 0baby, 1用户,3意见反馈

	public ChoosePhotoClick(ChoosePhotoAction photoAction, NativeAction action)
	{
		this.PhotoAction = photoAction;
		this.Action = action;
	}
	

	public NativeAction GetAction
	{
		get => Action;
		set => Action = value;
	}
	public ChoosePhotoAction GetPhotoAction
	{
		get => PhotoAction;
		set => PhotoAction = value;
	}
}


public class TipConfirmClick
{
	private TipAction Action;

	public TipConfirmClick(TipAction action)
	{
		this.Action = action;
	}

	public TipAction GetAction
	{
		get => Action;
		set => Action = value;
	}
}


public class ScanQRResult
{
	private string scanResult;

	public ScanQRResult(string scanResult)
	{
		this.scanResult = scanResult;
	}

	public string ScanResult
	{
		get => scanResult;
		set => scanResult = value;
	}
}


public class SelectAddressDate
{
	private ProvinceModel Date;

	public SelectAddressDate(ProvinceModel date)
	{
		this.Date = date;
	}
	

	public ProvinceModel GetDate
	{
		get => Date;
		set => Date = value;
	}
}

public class UpdateAddressDate
{
	private bool isUpdate;

	public UpdateAddressDate(bool isUpdate)
	{
		this.isUpdate = isUpdate;
	}
	

	public bool GetIsUpdate
	{
		get => isUpdate;
		set => isUpdate = value;
	}
}

public class UpdateBaseInfoDate
{
	private bool isUpdate;

	public UpdateBaseInfoDate(bool isUpdate)
	{
		this.isUpdate = isUpdate;
	}
	

	public bool GetIsUpdate
	{
		get => isUpdate;
		set => isUpdate = value;
	}
}

public class UpdateUserDate
{
	private bool isUpdate;

	public UpdateUserDate(bool isUpdate)
	{
		this.isUpdate = isUpdate;
	}
	

	public bool GetIsUpdate
	{
		get => isUpdate;
		set => isUpdate = value;
	}
}

public class RecordPath{
	
	private string _path;

	public RecordPath(string path)
	{
		_path = path;
	}

	public string Path
	{
		get => _path;
		set => _path = value;
	}
}



public class UpdateVol
{
	private string message;

	public UpdateVol(string message)
	{
		this.message = message;
	}

	public string Message
	{
		get => message;
		set => message = value;
	}
}

public class SSIDResult
{
	private string message;

	public SSIDResult(string message)
	{
		this.message = message;
	}

	public string Message
	{
		get => message;
		set => message = value;
	}
}


public class ExchangeOrderAddress
{
	public AddressInfoModel.AddressInfo AddressInfo
	{
		get => addressInfo;
		set => addressInfo = value;
	}
 
	private AddressInfoModel.AddressInfo addressInfo;

	public ExchangeOrderAddress(AddressInfoModel.AddressInfo addressInfo)
	{
		this.addressInfo = addressInfo;
	}
}

public class CollectGiftBoxPanelClosed
{
	private CollectGiftBoxType _CollectGiftBoxType;

	public CollectGiftBoxPanelClosed(CollectGiftBoxType collectGiftBoxType)
	{
		_CollectGiftBoxType = collectGiftBoxType;
	}

	public CollectGiftBoxType CollectGiftBoxType
	{
		get => _CollectGiftBoxType;
		set => _CollectGiftBoxType = value;
	}
}


public class CloseBindCheckWIFI
{
	
}

public class MainPanelGuideDismiss
{
	
}

public class LottieAnimationFinish
{
	
}


public class AttendanceSuccess
{
	private bool isSuccess;

	public AttendanceSuccess(bool isSuccess)
	{
		this.isSuccess = isSuccess;
	}

	public bool IsSuccess
	{
		get => isSuccess;
		set => isSuccess = value;
	}
}


public class VideoCompressResult
{
	private VideoCompressModel model;

	public VideoCompressResult(VideoCompressModel model)
	{
		this.model = model;
	}

	public VideoCompressModel Model
	{
		get => model;
		set => model = value;
	}
}

public class ShareResult
{
	private string status;

	public ShareResult(string status)
	{
		this.status = status;
	}

	public string Status
	{
		get => status;
		set => status = value;
	}
}