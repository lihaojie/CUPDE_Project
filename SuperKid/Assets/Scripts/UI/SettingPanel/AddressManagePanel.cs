/****************************************************************************
 * 2020.4 prada
 ****************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using SuperKid.Utils;
using UniRx;

namespace SuperKid
{
	public partial class AddressManagePanel : UIElement
	{
		private ResLoader mResLoader = ResLoader.Allocate();

		private void Awake()
		{
			SimpleEventSystem.GetEvent<UpdateAddressDate>()
				.Subscribe(_ =>
				{
					Log.I("_.GetDate: " + _.GetIsUpdate);
					if (_.GetIsUpdate)
					{
						StartRequestForGetAddress();
					}
				}).AddTo(this);
			StartRequestForGetAddress();
		}

		private void OnEnable()
		{
		}

		private void StartRequestForGetAddress()
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			if (PlayerPrefsUtil.UserInfo == null)
			{
				CommonUtil.toast("数据异常，请重新登录");
				return;
			}
			param.Add("babyId",PlayerPrefsUtil.UserInfo.relBabyId);
			param.Add("pageSize",500);
			param.Add("pageNum",1);
			HttpUtil.PostWithSign<AddressInfoModel>(UrlConst.BabyAddressListPage,param)
				.Subscribe(model =>
				{
					Log.I(model.ToJson());
					Content.DestroyAllChild();
					foreach (var modelRecord in model.records)
					{
						//SettingAddressItemPrefab SettingAddAddressPrefab
						
						mResLoader.LoadSync<GameObject>("SettingAddressItemPrefab")
							.Instantiate()
							.transform
							.LocalIdentity()
							.Parent(Content) 
							.LocalScale(1,1,1)
							.ApplySelfTo(game =>
							{
								game.Find("ItemAddress/TvName").GetComponent<Text>().text 
									= modelRecord.consignee + " " + modelRecord.consigneeMobile;
								game.Find("ItemAddress/TvAddress").GetComponent<Text>().text 
									= modelRecord.provinceName 
									  + modelRecord.cityName 
										+ modelRecord.areaName 
										+ modelRecord.consigneeAddress;
								if (modelRecord.isDefault == 1)
								{
									game.Find("ItemAddress/ImgDefault").GetComponent<Image>().gameObject.SetActive(true);
								}
								else
								{
									game.Find("ItemAddress/ImgDefault").GetComponent<Image>().gameObject.SetActive(false);
								}
								game.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(() =>
								{
									AudioManager.PlaySound("Button_Audio");
									UIMgr.OpenPanel<AddressEditPanel>(new AddressEditPanelData()
									{
										AddressInfo = modelRecord
									}, UITransitionType.CIRCLE);
								});
							})
							.Show();
					}
					mResLoader.LoadSync<GameObject>("SettingAddAddressPrefab")
						.Instantiate()
						.transform
						.LocalIdentity()
						.Parent(Content) 
						.LocalScale(1,1,1)
						.ApplySelfTo(game =>
						{
							game.Find("BtnAdd").GetComponent<Button>().onClick.AddListener(() =>
							{
								AudioManager.PlaySound("Button_Audio");
								UIMgr.OpenPanel<AddressEditPanel>(new AddressEditPanelData(), UITransitionType.CIRCLE);
							});
							
						})
						.Show();
				}, e =>
				{
					if (e is HttpException)
					{
						HttpException http = e as HttpException;
						Log.E("弹吐司" + http.Message);
					}
				}).AddTo(this);
		}
		
		protected override void OnBeforeDestroy()
		{
			mResLoader.Recycle2Cache();
			mResLoader = null;
		}
		
	}
}