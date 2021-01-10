package main

import (
	"sync"
)

const (
	maximumPlayersInQueue int = 8
)

var playersInQueue []*player = make([]*player, 8)

var queueMutex sync.RWMutex = sync.RWMutex{}

func getMatchmakingGeneralErrors(p *player) string {
	if !p.ableToPlay {
		return "Player's isAbleToPlay field is set to false!"
	}

	if p.activeGame != nil {
		return "Player is already in a game!"
	}

	return ""
}

func leaveQueue(p *player) string {

	queueMutex.Lock()

	err := getMatchmakingGeneralErrors(p)

	if err != "" {
		return err
	}

	if !queueContainsPlayer(p) {
		return "The player is not queued up!"
	}

	//leave queue logic

	queueMutex.Unlock()

	return ""
}

func queueUp(p *player) string {

	queueMutex.Lock()

	err := getMatchmakingGeneralErrors(p)

	if err != "" {
		return err
	}

	if len(playersInQueue) == maximumPlayersInQueue {
		return "The queue is full!"
	}

	//queue up logic

	queueMutex.Unlock()

	return ""
}

func queueContainsPlayer(p *player) bool {
	for _, c := range playersInQueue {
		if c == p {
			return true
		}
	}
	return false
}
