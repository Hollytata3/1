using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Xianxiao
{
    [RequireComponent(typeof(Button))]
    public class UIActionButton : MonoBehaviour, IUIElement<BaseCommand, UnityAction>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private Tooltip tooltip;

        private bool isActive;
        private RectTransform rectTransform;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            rectTransform = GetComponent<RectTransform>();
            Disable();
        }

        public void EnableFor(BaseCommand command, UnityAction onClick)
        {
            button.onClick.RemoveAllListeners();
            SetIcon(command.Icon);
            button.interactable = !command.IsLocked(new CommandContext());
            button.onClick.AddListener(onClick);
            isActive = true;

            if (tooltip != null)
            {
                tooltip.SetText(GetTooltipText(command));
            }
        }

        public void Disable()
        {
            SetIcon(null);
            button.interactable = false;
            button.onClick.RemoveAllListeners();
            isActive = false;
            CancelInvoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isActive)
            {
                Invoke(nameof(ShowTooltip), tooltip.HoverDelay);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltip != null)
            {
                tooltip.Hide();
            }
            CancelInvoke();
        }

        private void ShowTooltip()
        {
            if (tooltip != null)
            {
                tooltip.Show();
                Vector2 targetCenter = rectTransform.position + new Vector3(
                rectTransform.rect.width * 0.35f,
                rectTransform.rect.height * 0.35f
                ); 
                Rect tooltipRect = tooltip.GetComponent<RectTransform>().rect;
                float halfWidth = tooltipRect.width * 0.3f;
                float halfHeight = tooltipRect.height * 0.3f; 
                float clampedX = Mathf.Clamp(
                    targetCenter.x,
                    halfWidth,
                    Screen.width - halfWidth
                );
                float clampedY = Mathf.Clamp(
                    targetCenter.y,
                    halfHeight,
                    Screen.height - halfHeight
                );
                tooltip.RectTransform.position = new Vector2(clampedX, clampedY);
            }
        }

        private void SetIcon(Sprite icon)
        {
            if (icon == null)
            {
                this.icon.enabled = false;
            }
            else
            {
                this.icon.sprite = icon;
                this.icon.enabled = true;
            }
        }

        private string GetTooltipText(BaseCommand command)
        {
            string tooltipText = command.Name + "\n";

            SupplyCostSO supplyCost = null;
            if (command is BuildUnitCommand unitCommand)
            {
                supplyCost = unitCommand.Unit.Cost;
            }
            else if (command is BuildBuildingCommand buildingCommand)
            {
                supplyCost = buildingCommand.building.Cost;
            }

            if (supplyCost != null)
            {
                if (supplyCost.Minerals > 0)
                {
                    tooltipText += $"{supplyCost.Minerals} minerals";
                }
                if (supplyCost.Gas > 0)
                {
                    tooltipText += $"{supplyCost.Gas} gas";
                }
            }

            return tooltipText;
        }
    }
}


