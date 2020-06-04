using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class mqttTest : MonoBehaviour {
	private MqttClient client;
	
	private string topic = "iot/82a7f16b226942c09303d835ee5f30cf/ai0000000FT00164/app-3687dede3f66445322bd3f9832e3d68e";
	// Use this for initialization
	void Start () {
		// create client instance 
		client = new MqttClient("mqtt.ai.tuling123.com",10883 , false , null ); 
		
		// register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		// string clientId = Guid.NewGuid().ToString(); 
		string clientId = "82a7f16b226942c09303d835ee5f30cf@ai0000000FT00164/app-1e20fe42883e782d6f5158acf30bfed4";
		client.Connect(clientId, "82a7f16b226942c09303d835ee5f30cf", "1ac67929");
		
		// subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); 

	}
	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 

		Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message)  );
	} 

	void OnGUI(){
		if ( GUI.Button (new Rect (20,40,80,20), "Level 1")) {
			Debug.Log("sending...");
			client.Publish("iot/82a7f16b226942c09303d835ee5f30cf/ai0000000FT00164", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
			client.Publish(topic, System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!群组"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
			Debug.Log("sent");
		}
	}
	// Update is called once per frame
	void Update () {



	}
}
