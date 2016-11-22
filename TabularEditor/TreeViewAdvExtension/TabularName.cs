﻿using Aga.Controls.Tree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aga.Controls.Tree.NodeControls;
using System.Windows.Forms;
using TabularEditor.TOMWrapper;

namespace TabularEditor.UI.Tree
{
    /// <summary>
    /// NodeControl that will return the translated name of the tabular object,
    /// given a specific Culture.
    /// </summary>
    public class TabularNodeTextBox : Aga.Controls.Tree.NodeControls.NodeTextBox
    {
        private UIController UI;

        public TabularNodeTextBox(UIController UI)
        {
            this.DrawText += TabularNodeTextBox_DrawText;
            this.UI = UI;
        }

        private void TabularNodeTextBox_DrawText(object sender, DrawEventArgs e)
        {
            e.TextColor = ((e.Node.Tag as IHideableObject)?.IsHidden ?? false) ? Color.Gray : e.Node.Tree.ForeColor;
        }

        public override string GetToolTip(TreeNodeAdv node)
        {
            var err = (node.Tag as IErrorMessageObject)?.ErrorMessage;
            return err ?? string.Empty;
        }

        public override object GetValue(TreeNodeAdv node)
        {
            var level = node?.Tag as Level;
            if(level != null)
            {
                return string.Format("{0} ({1})", level.GetName(UI.Tree?.Culture), level.Column.GetName(UI.Tree?.Culture));
            }
            return (node?.Tag as ITabularNamedObject)?.GetName(UI.Tree?.Culture);
        }

        public override void SetValue(TreeNodeAdv node, object value)
        {
            (node.Tag as ITabularNamedObject).SetName((string)value, UI.Tree?.Culture);
        }

        protected override Control CreateEditor(TreeNodeAdv node)
        {
            var ctr = base.CreateEditor(node) as TextBox;
            if(node?.Tag is Level)
            {
                ctr.Text = (node.Tag as Level).Name;
            }
            return ctr;
        }
    }
}