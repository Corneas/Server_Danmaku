using UnityEngine;
using System.Collections;




public class MouseSerializer : Serializer
{

	public bool Serialize(KeyData packet)
	{
		// 각 요소를 차례로 시리얼라이즈합니다.
		bool ret = true;
		ret &= Serialize(packet.frame);	
		ret &= Serialize(packet.horizontal); ;
		ret &= Serialize(packet.vertical);
		ret &= Serialize(packet.inputSkill1);
		ret &= Serialize(packet.inputSkill2);
		ret &= Serialize(packet.inputShift);
		
		return ret;
	}

	public bool Deserialize(byte[] data, ref KeyData serialized)
	{
		// 데이터의 요소별로 디시리얼라이즈합니다.
		// 디시리얼라이즈할 데이터를 설정합니다.
		bool ret = SetDeserializedData(data);
		if (ret == false) {
			return false;
		}
		
		// 데이터의 요소별로 디시리얼라이즈합니다.
		ret &= Deserialize(ref serialized.frame);
		ret &= Deserialize(ref serialized.horizontal);
		ret &= Deserialize(ref serialized.vertical);
		ret &= Deserialize(ref serialized.inputSkill1);
		ret &= Deserialize(ref serialized.inputSkill2);
		ret &= Deserialize(ref serialized.inputShift);
		//ret &= Deserialize(ref serialized.mouseButtonLeft);
		//ret &= Deserialize(ref serialized.mouseButtonRight);
		//ret &= Deserialize(ref serialized.mousePositionX);
		//ret &= Deserialize(ref serialized.mousePositionY);
		//ret &= Deserialize(ref serialized.mousePositionZ);
		
		return ret;
	}
}



public class InputSerializer : Serializer
{
	public bool Serialize(InputData data)
	{
		// 기존 데이터를 클리어합니다.
		Clear();
		
		// 각 요소를 차례로 시리얼라이즈합니다.
		bool ret = true;
		ret &= Serialize(data.count);	
		ret &= Serialize(data.flag);

		MouseSerializer mouse = new MouseSerializer();
		
		for (int i = 0; i < data.keyData.Length; ++i) {
			mouse.Clear();
			bool ans = mouse.Serialize(data.keyData[i]);
			if (ans == false) {
				return false;
			}
			
			byte[] buffer = mouse.GetSerializedData();
			ret &= Serialize(buffer, buffer.Length);
		}
		
		return ret;
	}

	public bool Deserialize(byte[] data, ref InputData serialized)
	{
		// 디시리얼라이즈할 데이터를 설정합니다.
		bool ret = SetDeserializedData(data);
		if (ret == false) {
			return false;
		}
		
		// 데이터 요소별로 디시리얼라이즈합니다.
		ret &= Deserialize(ref serialized.count);
		ret &= Deserialize(ref serialized.flag);

		// 디시리얼라이즈 후의 버퍼 크기를 구합니다.
		MouseSerializer key = new MouseSerializer();
		KeyData kd = new KeyData();
		key.Serialize(kd);
		byte[] buf= key.GetSerializedData();
		int size = buf.Length;
		
		serialized.keyData = new KeyData[serialized.count];
		for (int i = 0; i < serialized.count; ++i) {
			serialized.keyData[i] = new KeyData();
		}
		
		for (int i = 0; i < serialized.count; ++i) {
			byte[] buffer = new byte[size];
			
			// mouseData의11프레임분의 데이터를 추출합니다.
			bool ans = Deserialize(ref buffer, size);
			if (ans == false) {
				return false;
			}

			ret &= key.Deserialize(buffer, ref kd);
			if (ret == false) {
				return false;
			}
			
			serialized.keyData[i] = kd;
		}
		
		return ret;
	}
}
