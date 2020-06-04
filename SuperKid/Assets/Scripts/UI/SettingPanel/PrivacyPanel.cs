/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class PrivacyPanel : UIElement
	{
		private string mPrivacy 
			= "朴和教育科技有限公司(以下简称“朴和教育”)尊重并保护所有使用服务用户的个人隐私权。为了给您提供更准确、更有个性化的服务，朴和教育会按照本隐私权政策的规定使用和披露您的个人信息。但朴和教育将以高度的勤勤勉勉、审慎义务对待这些信息。除本隐私权政策另有规定外，在未征得您事先许可的情况下,朴和教育不会将这些信息对外披露或向第三方提供。朴和教育会不时更新本隐私权政策。您在同意朴和教育服务用户隐私声明之时，即视为您已经同意本隐私权政策全部内容。本隐私权政策属于朴和教育服务使用协议不可分割的一部分。" +
              "\n1.适用范围" +
              "\na) 在您注册朴和教育帐号时，您根据朴和教育要求提供的个人注册信息;" +
              "\nb) 在您使用朴和教育网络服务，或访问朴和教育平台网页时，朴和教育自动接收并记录您的浏览器和计算机上的信息，包括但不限于您的IP地址、浏览器的类型、使用的语言、访问日期和时间、软硬件特征信息以及您需求的网页记录等数据;" +
              "\nc) 朴和教育通过合法途径从商业伙伴处取得用户个人数据。" +
              "\n2.您需要了解并同意，以下信息不适用本隐私权政策:" +
              "\na) 您在使用朴和教育平台提供的搜索服务时输入的关键字信息;" +
              "\nb) 朴和教育收集到的您在朴和教育发布的有关信息数据，包括但不限于参与活动、成交信息及评价详情;" +
              "\nc) 违反法律规定或违反朴和教育规则行为及朴和教育已对您采取的措施。" +
              "\n3.信息使用" +
              "\na) 朴和教育不会向任何无关第三方提供、出售、出租、分享或交易您的个人信息， 除非事先得到您的许可，或该第三方和朴和教育(含朴和教育关联公司)单独或共同为您提供服务，且在该服务结束后，其将被禁止访问包括其以前能够访问的所有这些资料。" +
              "\nb) 朴和教育亦不允许任何第三方以任何手段收集、编辑、出售或者无偿传播您的个人信息。任何朴和教育平台用户如从事上述活动，一经发现，朴和教育有权立即终止与该用户的服务协议。" +
              "\nc) 为服务用户为目的，朴和教育可能通过使用您的个人信息，向您提供您感兴趣的信息，包括但不限于向您发出产品和服务信息，或者与朴和教育合作伙伴共享信息 以便他们向您发送有关其产品和服务的信息(后者需要您的事先同意)。" +
              "\n4. 信息披露在如下情况下，朴和教育将依据您的个人意愿或法律的规定全部或部分披露露您的个人信息:" +
              "\na) 经您事先同意，向第三方披露露;" +
              "\nb) 为提供您所要求的产品和服务，而必须和第三方分享您的个人信息;" +
              "\nc) 根据法律的有关规定，或者行政或司法机构的要求，向第三方或者行政、司法机构披露;" +
              "\nd) 如您出现违反中国有关法律、法规或者朴和教育服务协相关规则的情况，需要向第三方披露;" +
              "\ne) 如您是适格的知识产权投诉人并已提起投诉，应被投诉人要求，向被投诉人披露，以便双方处理可能的权利纠纷;" +
              "\nf) 在朴和教育平台上创建的某一交易中，如交易任何一方履行或部分履行了交易义务并提出信息披露请求的，朴和教育有权决定向该用户提供其交易对方的联络方式等必要信息，以促成交易的完成或纠纷的解决。" +
              "\ng) 其它朴和教育根据法律、法规或者网站政策认为合适的披露。" +
              "\n5.信息存储和交换" +
              "\na)朴和教育收集的有关您的信息和资料将保存在朴和教育及(或)其关联公司的服务器上，这些信息和资料可能传送至您所在国家、地区或朴和教育收集信息和资料所在地的境外并在境外被访问、存储和展示。" +
              "\n6. Cookie的使用" +
              "\na) 在您未拒绝接受cookies的情况下，朴和教育会在您的计算机上设定或取用cookies，以便您能登录或使用依赖于cookies的朴和教育平台服务或功能。朴和教育使用cookies可为您提供更加周到的个性化服务，包括推广服务。" +
              "\nb) 您有权选择接受或拒绝接受cookies。您可以通过修改浏览器设置的方式拒绝接受cookies。但如果您选择拒绝接受cookies，则您可能无法登录或使用依赖于cookies的朴和教育网络服务或功能。" +
              "\nc) 通过朴和教育所设cookies所取得的有关信息，将适用本政策。" +
              "\n7.信息安全" +
              "\na) 朴和教育帐号均有安全保护功能，请妥善保管您的用户名及密码信息。朴和教育将通过对用户密码进行加密等安全措施确保您的信息不丢失，不被滥用和变造。尽管有前述安全措施，但同时也请您注意，在信息网络上不存在“完善的安全措施”。" +
              "\nb) 在使用朴和教育网络服务进行网上交易时，您不可避免的要向交易对方或潜在的交易对方披露自己的个人信息，如联络方式或者邮政地址。请您妥善保护自己的个人信息，仅在必要的情形下向他人提供。如您发现自己的个人信息泄密，尤其是朴和教育用户名及密码发生泄露，请您立即联络朴和教育客服，以便朴和教育采取相应措施。" +
              "\n8.本隐私政策的更改" +
              "\na) 如果决定更改隐私政策，我们会在本政策中、朴和教育网站中以及我们认为适当的位置发布这些更改，以便您了解我们如何收集、使用您的个人信息，哪些人可以访问这些信息，以及在什么情况下我们会透露这些信息。" +
              "\nb) 朴和教育保留随时修改本政策的权利，因此请经常查看。如对本政策作出重大更改，朴和教育会通过网站通知的形式告知。" +
              "\n9.如何联系我们" +
              "\n如果您对本隐私政策有任何疑问、意见或建议，通过以下方式与我们联系:" +
              "\n公司名称：朴和教育科技有限公司" +
              "\n注册地址：天津市临港经济区临港怡湾广场3-208-05/06室" +
              "\n常用办公地址：北京市海淀区丹棱街18号创富大厦7层" +
              "\n邮箱：ph-itservice@pxjy.com" +
              "\n联系电话：010-82580100-8192";

		private void OnEnable()
		{
			TextPrivacy.text = mPrivacy;
		}

		private void OnDisable()
		{
			TextPrivacy.text = "";

		}

		protected override void OnBeforeDestroy()
		{
		}
	}
}