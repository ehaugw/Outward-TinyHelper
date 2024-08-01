using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Tasks.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    //public static class InventoryExtension
    //{
    //    public static Item GetItemWithEnchantmentFromInventory(this CharacterInventory inventory, int itemID, int enchantmentID)
    //    {
    //        return null;
    //    }
    //}
    //public class Condition_SimpleOwnsItemEnchanted : Condition_SimpleOwnsItem
    //{
    //    public int enchant;
    //    protected override bool OnCheck()
    //    {
    //        if (character.value == null || item.value == null)
    //        {
    //            return false;
    //        }

    //        if (character.value.Cheats.OwnsQuestItems)
    //        {
    //            return true;
    //        }

    //        return character.value.Inventory.OwnsItem(item.value.ItemID, minAmount.value);
    //    }
    //}

    public class TinyDialogueManager
    {
        public static GameObject AssignMerchantTemplate(Transform parentTransform)
        {
            var trainerTemplate = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("editor/templates/MerchantTemplate"));
            trainerTemplate.transform.parent = parentTransform;
            trainerTemplate.transform.position = parentTransform.position;
            trainerTemplate.transform.rotation = parentTransform.rotation;

            return trainerTemplate;
        }

        public static GameObject AssignTrainerTemplate(Transform parentTransform)
        {
            var trainerTemplate = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("editor/templates/TrainerTemplate"));
            trainerTemplate.transform.parent = parentTransform;
            trainerTemplate.transform.position = parentTransform.position;
            trainerTemplate.transform.rotation = parentTransform.rotation;

            return trainerTemplate;
        }

        public static DialogueActor SetDialogueActorName(GameObject dialogueGameObject, string name)
        {
            var actor = dialogueGameObject.GetComponentInChildren<DialogueActor>();
            actor.SetName(name);
            return actor;
        }

        public static Merchant SetMerchant(GameObject merchantTemplate, UID merchantUID)
        {
            // get "Trainer" component, and set the SkillTreeUID to our custom tree UID
            Merchant merchant = merchantTemplate.GetComponentInChildren<Merchant>();
            return merchant;
        }

        public static Trainer SetTrainerSkillTree(GameObject trainerTemplate, UID skillTreeUID)
        {
            // get "Trainer" component, and set the SkillTreeUID to our custom tree UID
            Trainer trainerComp = trainerTemplate.GetComponentInChildren<Trainer>();
            At.SetValue(skillTreeUID, typeof(Trainer), trainerComp, "m_skillTreeUID");
            return trainerComp;
        }

        public static Graph GetDialogueGraph(GameObject trainerTemplate)
        {
            var graphController = trainerTemplate.GetComponentInChildren<DialogueTreeController>();
            var graph = (graphController as GraphOwner<DialogueTreeExt>).graph;
            return graph;
        }

        public static void SetActorReference(Graph graph, DialogueActor actor)
        {
            // the template comes with an empty ActorParameter, we can use that for our NPC actor.
            var actors = At.GetValue(typeof(DialogueTree), graph as DialogueTree, "_actorParameters") as List<DialogueTree.ActorParameter>;
            actors[0].actor = actor;
            actors[0].name = actor.name;
        }
        
        public static ActionNode MakeMerchantDialogueAction(Graph graph, Merchant merchant)
        {
            // the template already has an action node for opening the Train menu. 
            //var openTrainer = graph.allNodes[1] as ActionNode;
            var openTrainer = graph.AddNode<ActionNode>();
            var action = new ShopDialogueAction()
            {
                Merchant = new BBParameter<Merchant>(merchant),
                PlayerCharacter = new BBParameter<Character>() { name = "gInstigator" },
            };
            openTrainer.action = action;
            return openTrainer;
        }

        public static ActionNode MakeTrainDialogueAction(Graph graph, Trainer trainer)
        {
            // the template already has an action node for opening the Train menu. 
            //var openTrainer = graph.allNodes[1] as ActionNode;
            var openTrainer = graph.AddNode<ActionNode>();
            var action = new TrainDialogueAction()
            {
                Trainer = new BBParameter<Trainer>(trainer),
                PlayerCharacter = new BBParameter<Character>() { name = "gInstigator" }
            };
            openTrainer.action = action;
            return openTrainer;
        }

        public static ActionNode MakeStartQuest(Graph graph, int questID)
        {
            //GIVE PROGRES START
            ActionNode actionNode = graph.AddNode<ActionNode>();
            var reward = new GiveQuest();
            actionNode.action = reward;
            reward.quest = new BBParameter<Quest>(ResourcesPrefabManager.Instance.GetItemPrefab(questID) as Quest);
            return actionNode;
        }

        public static ActionNode MakeQuestEvent(Graph graph, string EventUID)
        {
            ActionNode actionNode = graph.AddNode<ActionNode>();
            var reward = new SendQuestEvent();
            actionNode.action = reward;
            reward.QuestEventRef = new QuestEventReference()
            {
                EventUID = EventUID,
            };
            return actionNode;
        }

        public static ConditionNode MakeEventOccuredCondition(Graph graph, string EventUID, int MinStack)
        {
            ConditionNode conditionNode = graph.AddNode<ConditionNode>();
            conditionNode.condition = new Condition_QuestEventOccured()
            {
                QuestEventRef = new QuestEventReference() { EventUID = EventUID },
                MinStack = MinStack,
            };
            return conditionNode;
        }

        public static ConditionNode MakeHasItemCondition(Graph graph, int itemID, int MinStack)
        {
            ConditionNode conditionNode = graph.AddNode<ConditionNode>();
            conditionNode.condition = new Condition_OwnsItem()
            {
                character = new BBParameter<Character>() { name = "gInstigator" },
                item = new BBParameter<ItemReference> (new ItemReference() { ItemID = itemID }),
                minAmount = new BBParameter<int>(MinStack),
                itemMustBeEquiped = new BBParameter<bool>(false),
                SaveMatchingContainerVariable = null,
            };
            return conditionNode;
        }

        public static ConditionNode MakeHasItemConditionSimple(Graph graph, int itemID, int MinStack, int enchantment = 0)
        {
            ConditionNode conditionNode = graph.AddNode<ConditionNode>();
            //conditionNode.condition = new Condition_SimpleOwnsItemEnchanted()
            conditionNode.condition = new Condition_SimpleOwnsItem()
            {
                character = new BBParameter<Character>() { name = "gInstigator" },
                item = new BBParameter<Item>(ResourcesPrefabManager.Instance.GetItemPrefab(itemID)),
                minAmount = new BBParameter<int>(MinStack),
                //enchant = enchantment
            };
            return conditionNode;
        }

        public static ActionNode MakeGiveItemReward(Graph graph, int itemID, GiveReward.Receiver receiver, int quantity = 1)
        {
            //GIVE PROGRES START
            ActionNode actionNode = graph.AddNode<ActionNode>();
            var reward = new GiveReward();
            actionNode.action = reward;
            reward.RewardReceiver = receiver;

            var itemQuantity = new NodeCanvas.Tasks.Actions.ItemQuantity();
            var itemRef = new ItemReference();
            itemRef.ItemID = itemID;
            itemQuantity.Item = new BBParameter<ItemReference>(itemRef);
            itemQuantity.Quantity = quantity;
            reward.ItemReward = new List<NodeCanvas.Tasks.Actions.ItemQuantity>() { itemQuantity };
            return actionNode;
        }

        public static ActionNode MakeResignItem(Graph graph, int itemID, GiveReward.Receiver provider, int quantity = 1, int enchantment = 0)
        {
            var actionNode = graph.AddNode<ActionNode>();
            actionNode.action = new RemoveItem()
            {
                fromCharacter = new BBParameter<Character>() { name = "gInstigator" },
                Items = new List<BBParameter<ItemReference>>
                {
                    new BBParameter<ItemReference>(new ItemReference() { ItemID = itemID }),
                },
                Amount = new List<BBParameter<int>> { new BBParameter<int>(quantity) },
            };
            return actionNode;
        }

        public static StatementNodeExt MakeStatementNode(Graph graph, string name, string statementText)
        {
            var statementNode = graph.AddNode<StatementNodeExt>();
            statementNode.statement = new Statement(statementText);
            statementNode.SetActorName(name);
            return statementNode;
        }
        
        public static MultipleChoiceNodeExt MakeMultipleChoiceNode(Graph graph, string[] statementTexts)
        {
            var multipleChoiceNode = graph.AddNode<MultipleChoiceNodeExt>();
            foreach (var statementText in statementTexts)
            {
                multipleChoiceNode.availableChoices.Add(new MultipleChoiceNodeExt.Choice { statement = new Statement { text = statementText } });
            }
            return multipleChoiceNode;
        }

        public static void ChainNodes(Graph graph, Node[] nodes)
        {
            Node prev = null;
            foreach (var node in nodes)
            {
                if (prev != null) graph.ConnectNodes(prev, node);
                prev = node;
            }
        }

        public static void ConnectMultipleChoices(Graph graph, Node baseNode, Node[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                graph.ConnectNodes(baseNode, nodes[i], i);
            }
        }
    }
}
