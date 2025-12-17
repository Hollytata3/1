using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Xianxiao;

namespace Xianxiao
{
    public class RuntimeUI : MonoBehaviour
    {
        [SerializeField] private ActionsUI actionsUI;
        [SerializeField] private BuildingBuildingUI buildingbuildingUI;
        [SerializeField] private UnitIconUI unitIconUI;
        [SerializeField] private SingleUnitSelectedUI singleUnitSelectedUI;

        private HashSet<AbstractCommandable> selectedUnits = new(12);

        private void Awake()
        {
            Bus<UnitSelectedEvent>.OnEvent += HandleUnitSelected;
            Bus<UnitDeselectedEvent>.OnEvent += HandleUnitDeselected;
            Bus<UnitDeathEvent>.OnEvent += HandleUnitDeath;
            Bus<SupplyEvent>.OnEvent += HandleSupplyChange;
        }
        private void Start()
        {
            actionsUI.Disable();
            buildingbuildingUI.Disable();
            unitIconUI.Disable();
        }

        private void OnDestroy()
        {
            Bus<UnitSelectedEvent>.OnEvent -= HandleUnitSelected;
            Bus<UnitDeselectedEvent>.OnEvent -= HandleUnitDeselected;
            Bus<UnitDeathEvent>.OnEvent -= HandleUnitDeath;
            Bus<SupplyEvent>.OnEvent -= HandleSupplyChange;
        }

        private void HandleUnitSelected(UnitSelectedEvent evt)
        {
            if (evt.Unit is AbstractCommandable commandable)
            {
                selectedUnits.Add(commandable);
                RefreshUI();
            }
        }

        private void HandleUnitDeselected(UnitDeselectedEvent evt)
        {
            if (evt.Unit is AbstractCommandable commandable)
            {
                selectedUnits.Remove(commandable);

                RefreshUI();
            }
        }

        private void RefreshUI()
        {
            if (selectedUnits.Count > 0)
            {
                actionsUI.EnableFor(selectedUnits);

                if (selectedUnits.Count == 1)
                {
                    AbstractCommandable commandable = selectedUnits.First();
                    unitIconUI.EnableFor(commandable);
                    singleUnitSelectedUI.EnableFor(commandable);

                    if (commandable is BaseBuilding building)

                    {
                        buildingbuildingUI.EnableFor(building);
                }
                else
                {
                    buildingbuildingUI.Disable();
                }
            }
            else
                {
                    unitIconUI.Disable();
                    singleUnitSelectedUI.Disable();
                    buildingbuildingUI.Disable();
                }
            }
            else

            {
                actionsUI.Disable();
                buildingbuildingUI.Disable();
                unitIconUI.Disable();
                singleUnitSelectedUI.Disable();

            }
        }

        private void HandleUnitDeath(UnitDeathEvent evt)
        {
            selectedUnits.Remove(evt.Unit);
            RefreshUI();
        }
        private void HandleSupplyChange(SupplyEvent evt)
        {
            actionsUI.EnableFor(selectedUnits);
        }
    } 
}

