using System.Runtime.InteropServices;

namespace ChromassProtocol
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Header
    {
        public int Length;         // 패킷 총 길이(Byte)

        public uint Id;             // 사용안함 0으로 설정 
        public uint Code;           // Packet code table 참조 (패킷 구분 코드) 
        public int Index;           // Event(Table 데이터)일 때의 Index	// 0 부터 보낸다. --> 어레이 1번에 저장하라
                                    // 제어기로 보낼때는 1번 어레이 부터 보내고 ,nEventIndex는 0부터 시작하라
                                    // Inlet 이나 Detector 등에서 설치된 위치를 표시함. (사용하지 않을 경우 0으로 설정) 
        public int SlotOffset;
        public int SlotSize;             // slot's size
    }
}