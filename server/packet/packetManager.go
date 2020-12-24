package packet

import (
	"reflect"
	"strconv"
)

type PacketHandler func(pd *PacketDataType, p *Packet)

type PacketDataType struct {
	Description string
	ID          uint16
	Primitives  []reflect.Kind
	Handler     *PacketHandler
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

func (pm *PacketManager) RegisterPacketDataType(p *PacketDataType) {
	_, exists := pm.packets[p.ID]
	if exists {
		panic("packet type of id " + strconv.Itoa(int(p.ID)) + " already exists!")
	}
	pm.packets[p.ID] = p
}
