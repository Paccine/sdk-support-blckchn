using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

namespace Atavism
{
    // Classe UGUIAbility é responsável pela interação do usuário com habilidades no UI
    public class UGUIAbility : MonoBehaviour, IPointerClickHandler
    {
        public int slotNum;  // Número do slot da habilidade
        public Text Name;  // Nome da habilidade no Unity UI
        public TextMeshProUGUI TMPName;  // Nome da habilidade no TextMeshPro UI
        public Text description;  // Descrição da habilidade no Unity UI
        public TextMeshProUGUI TMPDescription;  // Descrição da habilidade no TextMeshPro UI
        public Text levelReq;  // Requisito de nível no Unity UI
        public TextMeshProUGUI TMPLevelReq;  // Requisito de nível no TextMeshPro UI
        public UGUIAbilitySlot abilitySlot;  // Slot da habilidade
        AtavismAbility ability;  // Instância de uma habilidade do Atavism
        bool mouseEntered = false;  // Booleano para verificar se o mouse está sobre a habilidade

        bool isClicked = false;

        
        void Start()
        {
            // ~ # P4DEVELOPMENTSTUDIO # ~ 
        }

        // Método para atualizar dados da habilidade no UI
        public void UpdateAbilityData(AtavismAbility ability)
        {
            this.ability = ability;
            abilitySlot.UpdateAbilityData(ability);  // Atualiza os dados do slot da habilidade

            // Se a habilidade é nula, limpa os campos do UI
            if (ability == null)
            {
                if (Name != null)
                    Name.text = "";
                if (TMPName != null)
                    TMPName.text = "";
                if (description != null)
                    description.text = "";
                if (TMPDescription != null)
                    TMPDescription.text = "";
                return;
            }

#if AT_I2LOC_PRESET
            // Atualiza campos do UI com tradução da habilidade caso haja um preset de localização I2
            if(description!=null) description.text = I2.Loc.LocalizationManager.GetTranslation("Ability/" + ability.tooltip);
            if(TMPDescription!=null) TMPDescription.text = I2.Loc.LocalizationManager.GetTranslation("Ability/" + ability.tooltip);
            if(Name!=null)   Name.text = I2.Loc.LocalizationManager.GetTranslation("Ability/" + ability.name);
            if(TMPName!=null)   TMPName.text = I2.Loc.LocalizationManager.GetTranslation("Ability/" + ability.name);
#else
            // Atualiza campos do UI com os dados da habilidade caso não haja um preset de localização I2
            if (Name != null)
                Name.text = ability.name;
            if (TMPName != null)
                TMPName.text = ability.name;
            if (description != null)
                description.text = ability.tooltip;
            if (TMPDescription != null)
                TMPDescription.text = ability.tooltip;
#endif
            // Atualiza requisitos de nível da habilidade
            if (levelReq != null)
            {
                levelReq.text = "";
                Skill skill = Skills.Instance.GetSkillOfAbility(ability.id);
                if (skill != null)
                {
                    for (int i = 0; i < skill.abilities.Count; i++)
                    {
                        if (skill.abilities[i] == ability.id)
                        {
                            levelReq.text = skill.abilityLevelReqs[i].ToString();
                            break;
                        }
                    }
                }
            }
            if (TMPLevelReq != null)
            {
                TMPLevelReq.text = "";
                Skill skill = Skills.Instance.GetSkillOfAbility(ability.id);
                if (skill != null)
                {
                    for (int i = 0; i < skill.abilities.Count; i++)
                    {
                        if (skill.abilities[i] == ability.id)
                        {
                            TMPLevelReq.text = skill.abilityLevelReqs[i].ToString();
                            break;
                        }
                    }
                }
            }

            // Verifica se o jogador conhece a habilidade e atualiza a cor do painel correspondente
            if (!Abilities.Instance.PlayerAbilities.Contains(ability))
            {
                GetComponent<Image>().color = Color.gray;  // Cor cinza caso o jogador não saiba a habilidade
            }
            else
            {
                GetComponent<Image>().color = Color.white;  // Cor branca caso o jogador saiba a habilidade
            }
        }

        // Método executado quando o mouse entra no elemento de habilidade
        public void OnPointerClick(PointerEventData eventData)
        {
            // Trocar o valor de isClicked cada vez que o mouse é clicado
            isClicked = !isClicked;

            // Se isClicked for verdadeiro, então é o "primeiro" clique, então execute a ação "OnPointerEnter"
            if (isClicked)
            {

            MouseEntered = true;

            }
            // Se isClicked for falso, então é o "segundo" clique, então execute a ação "OnPointerExit"
            else
            {

            MouseEntered = false;

            }
        }

        // Propriedade para checar se o mouse está sobre o elemento de habilidade e mostrar tooltip
        public bool MouseEntered
        {
            get
            {
                return mouseEntered;
            }
            set
            {
                mouseEntered = value;
                if (mouseEntered && ability != null)
                {
                    ability.ShowTooltip(gameObject);  // Mostra tooltip se o mouse estiver sobre o elemento
                }
                else
                {
                    HideTooltip();  // Esconde tooltip caso contrário
                }
            }
        }

        // Método para esconder a tooltip
        void HideTooltip()
        {
            UGUITooltip.Instance.Hide();  // Esconde a instância da tooltip
        }
    }
}