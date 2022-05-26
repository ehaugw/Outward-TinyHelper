using SideLoader;

namespace TinyHelper
{
    public class CasualStagger : Effect
    {
        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Stagger(_affectedCharacter);
        }
        public static void Stagger(Character character)
        {
            At.SetValue(Character.HurtType.Knockback, typeof(Character), character, "m_hurtType");
            character.Animator.SetTrigger("Knockback");
            character.ForceCancel(false, true);
            character.Invoke("DelayedForceCancel", 0.3f);
            if (character.CharacterUI) character.CharacterUI.OnPlayerKnocked();
        }
    }
}
