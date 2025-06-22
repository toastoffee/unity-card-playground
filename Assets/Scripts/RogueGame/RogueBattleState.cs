using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public partial class RogueBattleState : UILiteState {
  public RogueBattleViewModel viewModel;

  public TextBinder text1;
  public SimpleListBinder<string> textList;


  public void Start() {
    viewModel = new RogueBattleViewModel();
    viewModel.name.Bind(text1);

    viewModel.name.value = "1234";

    viewModel.list.value = new();
    for (int i = 0; i < 10; i++) {
      var item = new DProp<string>();
      item.value = i.ToString();
      viewModel.list.value.Add(item);
    }

    textList.binderCreateHandler = (param) => {
      var binder = new TextBinder();
      binder.SetComp(param.inst.GetComponent<TMPro.TMP_Text>());
      return binder;
    };
    viewModel.list.Bind(textList);
  }
}

public class RogueBattleViewModel {
  public DProp<string> name = new();
  public DProp<List<DProp<string>>> list = new();
}
