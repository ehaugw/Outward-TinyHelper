namespace TinyHelper
{
    class InputEnablerEffect : Effect
    {
        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (this.m_parentStatusEffect is StatusEffect parent)
            {
                if ((_affectedCharacter?.AnimMoveSqMagnitude ?? 0) > 0.1 && parent.Age > 0.5)
                {
                    _affectedCharacter.StatusEffectMngr?.CleanseStatusEffect(parent.IdentifierName);
                    _affectedCharacter.ForceCancel(true, true);
                }
            }
        }
    }
}
