using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardList
{
    public List<NormalCardBase> normalCardBaseList = new List<NormalCardBase>();
    public List<RabbitCardBase> rabbitCardBaseList = new List<RabbitCardBase>();
    public List<CarrotCardBase> carrotCardBaseList = new List<CarrotCardBase>();
    public List<AddCardBase> addCardBaseList = new List<AddCardBase>();
    public List<NormalCardBase> currentCardBaseList = new List<NormalCardBase>();
}
