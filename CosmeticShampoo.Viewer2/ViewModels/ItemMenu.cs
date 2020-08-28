using System.Collections.Generic;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;

namespace CosmeticShampoo.Viewer2.ViewModels
{
    

    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems, PackIconMaterialKind icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }

        public ItemMenu(string header, PackIconMaterialKind icon, UserControl screen = null)
        {
            Header = header;
            Screen = screen;
            Icon = icon;
        }

        public string Header { get; private set; }
        public PackIconMaterialKind Icon { get; private set; }

        public List<SubItem> SubItems { get; private set; }

        public UserControl Screen { get; private set; }
    }
}
