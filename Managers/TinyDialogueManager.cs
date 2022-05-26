using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class TinyDialogueManager
    {
        public static GameObject AssignTrainerTemplate(Transform parentTransform)
        {
            var trainerTemplate = UnityEngine.Object.Instantiate(Resources.Load("editor/templates/TrainerTemplate")) as GameObject;
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

        public static ActionNode MakeTrainDialogueAction(Graph graph, Trainer trainer)
        {
            // the template already has an action node for opening the Train menu. 
            var openTrainer = graph.allNodes[1] as ActionNode;
            (openTrainer.action as TrainDialogueAction).Trainer = new BBParameter<Trainer>(trainer);
            return openTrainer;
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
    }
}
