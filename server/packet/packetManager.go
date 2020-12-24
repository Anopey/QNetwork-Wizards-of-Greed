package packet

import (
	"reflect"
	"strconv"
)

type PacketHandler func(pd *PacketDataType, p *Packet)

type PacketDataType struct {
	id         uint16
	primitives []reflect.Type
	handler    *PacketHandler
}

type PacketManager struct {
	packets map[uint16]*PacketDataType
}

func NewPacketManager() *PacketManager {
	pm := PacketManager{
		packets: make(map[uint16]*PacketDataType),
	}
	return &pm
}

func (pm *PacketManager) RegisterPacketDataType(p PacketDataType) {
	_, exists := pm.packets[p.id]
	if exists {
		panic("packet type of id " + strconv.Itoa(int(p.id)) + " already exists!")
	}
	pm.packets[p.id] = &p
}

