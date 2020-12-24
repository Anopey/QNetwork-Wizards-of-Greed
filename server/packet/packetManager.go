package packet

import "reflect"

type PacketHandler func(pd *PacketData, p *Packet)

type PacketData struct {
	id         int
	primitives []reflect.Type
	handler    *PacketHandler
}

type PacketManager struct {
	packets map[uint16]*PacketData
}

func NewPacketManager() *PacketManager {
	pm := PacketManager{
		packets: make(map[uint16]*PacketData),
	}
	return &pm
}
