using System;
using System.Collections.Generic;

public static class GameEvents {
	private static GameEvent[] events = new GameEvent[] {
		new GameEvent() {
			id = "finca",
			prompt = "You approach your home, built with the sweat of your brow - years of hard work. Now you have no choice but leave it behind.",
			options = new GameEventOption[] {
				new GameEventOption() { description = "Let's hurry and pack whatever we can fit in Rosita, and on our own backs" }
			}
		},
		new GameEvent() {
			id = "uramita",
			prompt = "You approach Uramita, a small town where your family used to live many years ago.",
			options = new GameEventOption[] {
				new GameEventOption() { description = "We can try to trade some of what we have here." }
			}
		},
		new GameEvent() {
			id = "dabeiba",
			prompt = "You approach Dabeiba, a trade center of the region growing increasingly dangerous.",
			options = new GameEventOption[] {
				new GameEventOption() { description = "We can try to trade some of what we have here." }
			}
		},
		new GameEvent() { id = "militia", prompt = "A group of people armed with old rifles and machetes approaches." +
			"\nOne man moves forward from the group while several men point to you with their guns." +
			"\n“You look like a god-damned chulavita!“", options = new GameEventOption[] {
				new GameEventOption() { id = "notAChula", description = "I’m not a chulavita, I swear! Please let me pass!" },
				new GameEventOption() { id = "proudChula", description = "And a proud one!" }
			}
		},
			new GameEvent() {
				id = "notAChula",
				prompt = "The man turns his back on you, and without looking gives a signal to his men after spitting to the ground.\n\n- Kill them.",
				options = new GameEventOption[] {
					new GameEventOption() { description = "Have Mercy!" }
				}
			},
				new GameEvent() {
					id = "notAChula1",
					prompt = "You see a chance to escape and shout for your family to run to the woods.\n\n"+
						"Chaos ensues as bullets fly. After a while you stop running and see who’s with you.\n\n"+
						"Sadly XXX is missing.",
					options = new GameEventOption[] {
						new GameEventOption() { description = "God rest his soul." }
					}
				},
				new GameEvent() {
					id = "notAChula2",
					prompt = "You see a chance to escape and shout for your family to run to the woods.\n\n"+
						"Chaos ensues as bullets fly. After a while you stop running and see who’s with you.\n\n"+
						"God is blessed, you are all able to regroup and continue.",
					options = new GameEventOption[] {
						new GameEventOption() { description = "Thank you, holy virgin." }
					}
				},
				new GameEvent() {
					id = "notAChula3",
					prompt = "You see a chance to escape and run to the woods.\n\n"+
						"After a while you stop running and notice you were hit. ",
					options = new GameEventOption[] {
						new GameEventOption() { description = "Thank you, holy virgin." }
					}
				},
		new GameEvent() { id = "casaquemada", prompt = "The house of our friends the Zapatas... Burnt by those who threatened us out of our home.", options = new GameEventOption[] {
			new GameEventOption() { description = "It's better not to look, kids, and just keep walking..." }
		}},
		new GameEvent() {
			id = "stolenAnimal",
			prompt = "After a good night’s sleep, you notice XXX is missing. After spending several hours looking for it, you decide it’s time to move on.",
			options = new GameEventOption[] {
				new GameEventOption() { description = "I should have been more careful." }
			}
		},
		new GameEvent() {
			id = "cart",
			prompt = "Ahead on the road you see what appears to be a cart. It has some food inside it that seems edible. The surrounding soil is covered by blood. There are no mules or horses, and no one answer your calls.",
			options = new GameEventOption[] {
				new GameEventOption() { description = "Load Rosita with all the food. [+20 food]" },
				new GameEventOption() { description = "Leave some food behind. Someone else may need it. [+10 food]" }
			}
		},
		new GameEvent() {
			id = "rioMelcocho",
			prompt = "Years ago, when looking at the crystalline waters of this river, at its colors, you’d think there can only exist peace." +
			"\nNowadays however, you only think how easily your family will get shot if you stop here.",
			options = new GameEventOption[] {
				new GameEventOption() { description = "Don't stop now! We must go!" }
			}
		},
		new GameEvent() {
			id = "finca2",
			prompt = "There was a time when you’d visit the family who lived here." +
			"\nBack then, you could trust your neighbors. You felt safe." +
			"\nNow, you’re afraid to ask for help. What if they think you are part of the Chulavitas?",
			options = new GameEventOption[] {
				new GameEventOption() { description = "People have been shot for far less..." }
			}
		},
		new GameEvent() { id = "house", prompt = "Your family aproaches what seems like an abandoned house." +
			"\nProbably another family displaced by violence." +
			"\n Should we search for food, risking to be taken as thieves and attacked?", options = new GameEventOption[] {
			new GameEventOption() { id = "steal", description = "Let's take the risk, we don't have a choice." },
			new GameEventOption() { description = "No. Let's continue down the road." },
		}},
		new GameEvent() { id = "house_food", prompt = "Luckily, we found some yuca in the kitchen.", options = new GameEventOption[] {
			new GameEventOption() { description = "Blessed be our mother the virgin." }
		}},
		new GameEvent() { id = "house_flee", prompt = "We are caught red-handed, and driven off the house by the machete.", options = new GameEventOption[] {
			new GameEventOption() { description = "Have mercy! Have mercy!" }
		}}
	};

	public static GameEvent Get(string id) {
		return Array.Find(events, e => e.id == id);
	}

	public static void OptionSelected (GameEvent currentEvent, GameEventOption option) {
		switch (currentEvent.id) {
			case "house":
				if (option.id == "steal") {
					int dice = UnityEngine.Random.Range(1, 4);
					if (dice < 3) {
						InventoryItem food = Expedition.i.inventory.Find(i => i.itemType == ItemType.FOOD);
						food.quantity = food.quantity + UnityEngine.Random.Range(5, 8);;
						GameUI.i.ShowEvent(GameEvents.Get("house_food"));
					} else {
						FamilyMember rando = Expedition.i.members[UnityEngine.Random.Range(0, Expedition.i.members.Count)];
						rando.TakeDamage(UnityEngine.Random.Range(2, 5));
						GameUI.i.ShowEvent(GameEvents.Get("house_flee"));
					}
					GameUI.i.UpdateStatus();
					return;
				}
				break;
			case "finca": case "uramita": case "dabeiba":
				GameUI.i.EventsDialog.Hide();
				GameUI.i.ShowTransfer(currentEvent.id);
				return;
			case "militia":
				if (option.id == "notAChula") {
					if (Expedition.i.GetHumans().Count == 1) {
						GameUI.i.ShowEvent(GameEvents.Get("notAChula3"));
						Expedition.i.GetHumans()[0].TakeDamage(UnityEngine.Random.Range(10, 30));
					} else if (UnityEngine.Random.Range(0, 100) < 20) {
						GameUI.i.ShowEvent(GameEvents.Get("notAChula2"));
					} else {
						FamilyMember rando = Expedition.i.RandomHuman();
						Expedition.i.Die(rando);
						GameUI.i.ShowPersonEvent(GameEvents.Get("notAChula1"), rando.memberName);
					}
				} else {
					GameUI.i.ShowGameOver("The man spits on your face before hitting you with the butt of his rifle. Everything blacks out.\n\n"+
					"When you wake up, the first thing you see is a crowd. You don’t see your family.\n\n"+
					"You try to move, but your hands are tied. A man grabs your hair and lifts your face. You see many people. Where are you? Your head hurts too much to think and the man speaks.\n\n"+
					"Look at the faces of the traitors! Know that even if the traitors are children or old people we don’t care! We will pacify this country, with fire and blood if needed!\n\n"+
					"Everything blacks out again. This time forever.");
				}
				return;
		}
		GameUI.i.EventsDialog.Hide();
		World.i.ResumeTime();
	}
}