package main

var playersInQueue []*player = make([]*player, 8)

func queueUp(p *player) string {

	if !p.ableToPlay {
		return "Player's isAbleToPlay field is set to false!"
	}

	return ""
}
