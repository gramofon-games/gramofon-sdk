#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public class GramofonWindow : OdinMenuEditorWindow
{
    [MenuItem("Gramofon SDK/Utility")]
    public static void ShowWindow()
    {
        EditorWindow editorWindow = GetWindow(typeof(GramofonWindow), true, "Gramofon SDK");
        editorWindow.position = GUIHelper.GetEditorWindowRect().AlignCenter(750, 0);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true) {};

        return tree;
    }
}
#endif