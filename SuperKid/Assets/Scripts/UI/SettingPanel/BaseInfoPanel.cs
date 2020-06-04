/****************************************************************************
 * 2020.4 爵色的MacBook Pro
 ****************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UniRx;


namespace SuperKid
{
	public partial class BaseInfoPanel : UIElement
	{
		public ListView listViewVertical;
		public ToggleGroup mToggleGroup;
		private ResLoader mResLoader  = ResLoader.Allocate();
		private UserInfoModel mUserInfoModel;
		private ItemInfoBaby itemInfoBaby;
		private ItemInfoUser itemInfoUser;
		private ItemInfoFamilyEmpty infoFamilyEmpty;
		private ItemInfoFamilyTitle infoFamilyTitle;
		private ItemInfoFamily familyItem;

		//ItemFamily
		
		private void Awake()
		{
			SimpleEventSystem.GetEvent<UpdateUserDate>()
				.Subscribe(_ =>
				{
					Log.I("_.GetDate: " + _.GetIsUpdate);
					if (_.GetIsUpdate)
					{
						InitData();
					}
				}).AddTo(this);
			
			SimpleEventSystem.GetEvent<UpdateBaseInfoDate>()
				.Subscribe(_ =>
				{
					Log.I("_.GetDate: " + _.GetIsUpdate);
					if (_.GetIsUpdate)
					{
						InitData();
					}
				}).AddTo(this);
			mToggleGroup = GetComponentInChildren<ToggleGroup>();
			InitData();
		}

		private void OnEnable()
		{
			
		}

		private void UpdateUserData()
		{
			mUserInfoModel = PlayerPrefsUtil.UserInfo;
			if (mUserInfoModel.IsNotNull() && mUserInfoModel.babyInfoVo.IsNotNull())
			{
				itemInfoBaby.SetContent(mUserInfoModel.babyInfoVo);
				itemInfoUser.SetContent(mUserInfoModel);
			}
		}
		
		private void InitData()
		{
			listViewVertical.RemoveAllItems();
			mUserInfoModel = PlayerPrefsUtil.UserInfo;
			if (mUserInfoModel.IsNotNull() && mUserInfoModel.babyInfoVo.IsNotNull())
			{
				AddBabyItem();
				AddUserItem();
			}
			if (PlayerPrefsUtil.GetDeviceId().IsNullOrEmpty())
			{
				AddFamilyEmptyItem();
			}
			else
			{
				StartRequestForFindMemberInfo();
			}
		}

		private void StartRequestForFindMemberInfo()
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("relBabyId",PlayerPrefsUtil.UserInfo.relBabyId);
			HttpUtil.GetWithSign<List<FamilyModel>>(UrlConst.FindMemberInfo,param)
				.Subscribe(response =>
				{
					Log.I(response.ToJson());
					if (response.IsNotNull())
					{
						int adminId = 0;
						foreach (var model in response)
						{
							if (model.isManager == 1)
							{
								adminId = model.id;
								break;
							}
						}
						if (adminId == PlayerPrefsUtil.GetUserId() && response.Count > 1)
						{
							AddFamilyTitleItem(true);
						}
						else
						{
							AddFamilyTitleItem(false);
						}
						foreach (var model in response)
						{
							AddItem(model);
						}
					}
					else
					{
						AddFamilyTitleItem(false);
					}
				}, e =>
					{
						if (e is HttpException)
						{
							HttpException http = e as HttpException;
							Log.E("弹吐司" + http.Message);
						}
					}).AddTo(this);
		}
		
		

		private void AddBabyItem()
		{
			mResLoader.LoadSync<GameObject>("ItemInfoBabyPrefab")
				.Instantiate()
				.transform
				.Identity()
				.ApplySelfTo(item =>
				{

					itemInfoBaby = item.GetComponent<ItemInfoBaby>();
					itemInfoBaby.SetContent(mUserInfoModel.babyInfoVo);
					listViewVertical.AddItem(item.gameObject);
				});
		}
		
		private void AddUserItem()
		{//ItemInfoUserPrefab
			mResLoader.LoadSync<GameObject>("ItemInfoUserPrefab")
				.Instantiate()
				.transform
				.Identity()
				.ApplySelfTo(item =>
				{
					itemInfoUser = item.GetComponent<ItemInfoUser>();
					itemInfoUser.SetContent(mUserInfoModel);
					listViewVertical.AddItem(item.gameObject);
				});
		}
		
		private void AddFamilyEmptyItem()
		{//ItemInfoUserPrefab
			mResLoader.LoadSync<GameObject>("ItemInfoFamilyEmptyPrefab")
				.Instantiate()
				.transform
				.Identity()
				.ApplySelfTo(item =>
				{
					infoFamilyEmpty = item.GetComponent<ItemInfoFamilyEmpty>();
					listViewVertical.AddItem(item.gameObject);
				});
		}

		private void AddFamilyTitleItem(bool hasOther)
		{
			mResLoader.LoadSync<GameObject>("ItemInfoFamilyTitlePrefab")
				.Instantiate()
				.transform
				.Identity()
				.ApplySelfTo(item =>
				{
					infoFamilyTitle = item.GetComponent<ItemInfoFamilyTitle>();
					infoFamilyTitle.setContent(hasOther);
					listViewVertical.AddItem(item.gameObject);
				});
		}

		
		private void AddItem(FamilyModel familyModel)
		{
			mResLoader.LoadSync<GameObject>("ItemInfoFamilyPrefab")
				.Instantiate()
				.transform
				.Identity()
				.ApplySelfTo(item =>
				{
					familyItem = item.GetComponent<ItemInfoFamily>();
					familyItem.SetContent(familyModel);
					listViewVertical.AddItem(item.gameObject);
				}); 
		}

		public void OnToggleClick(FamilyItem item, bool value)  
		{
			if (value)
			{
				Log.I("index: " + item.index);
			}
			Debug.Log("toggle change " + (value ? "On" : "Off"));  
		}  
		
		protected override void OnBeforeDestroy()
		{
			mResLoader.Recycle2Cache();
			mResLoader = null;
		}
	}
}