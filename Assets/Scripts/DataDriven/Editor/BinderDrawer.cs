using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using static Unity.Burst.Intrinsics.X86.Avx;

public class BinderDrawer<Comp> : OdinValueDrawer<CompHolder<Comp>> where Comp : UnityEngine.Component {
  protected override void DrawPropertyLayout(GUIContent label) {
    // 获取 DrawTest<T> 实例
    var entry = this.ValueEntry;
    var drawTest = entry.SmartValue;

    // 获取 Value 属性的 PropertyTree
    var property = entry.Property.Children["comp"];

    // 直接绘制 T 类型的 Value 字段（复用 Odin 对 T 的默认绘制逻辑）
    property.Draw(label ?? GUIContent.none);

    // 同步修改回原始对象（避免值拷贝问题）
    entry.SmartValue = drawTest;
  }
}

public class BinderDrawer2<T, Comp> : OdinValueDrawer<T> where T : CompHolder<Comp> where Comp : Component {
  protected override void DrawPropertyLayout(GUIContent label) {
    // 获取 DrawTest<T> 实例
    var entry = this.ValueEntry;
    var drawTest = entry.SmartValue;

    // 获取 Value 属性的 PropertyTree
    var property = entry.Property.Children["comp"];

    // 直接绘制 T 类型的 Value 字段（复用 Odin 对 T 的默认绘制逻辑）
    property.Draw(label ?? GUIContent.none);

    // 同步修改回原始对象（避免值拷贝问题）
    entry.SmartValue = drawTest;
  }
}

