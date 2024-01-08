using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class AStar
{
    #region 找出可移动范围
    public static void MoveableArea(Character unit, int startIndex, float movePower,
        GMap map, Dictionary<int, AStarNode> nodeList, List<int> enemyList)
    {
        AStarNode astarNode = (!nodeList.ContainsKey(startIndex)) ? null : nodeList[startIndex];
        foreach (int neighborIndex in map.neighborhood(startIndex))
        {
            if (neighborIndex >= 0)
            {
                if (astarNode == null || neighborIndex != astarNode.prevNodeIndex)
                {
                    //if (neighborIndex != unit.tileIndex)
                    {
                        float moveCost = AStar.MoveCost(unit, startIndex, neighborIndex, map, enemyList);
                        float totalMoveCost = moveCost + ((astarNode != null) ? astarNode.totalCost : 0f);
                        float curMovePower = movePower - moveCost;
                        //在攻击模式就把所有都进行考虑
                        if (curMovePower >= 0f && (!nodeList.ContainsKey(neighborIndex) || nodeList[neighborIndex].totalCost > totalMoveCost))
                        {
                            nodeList[neighborIndex] = new AStarNode(neighborIndex, startIndex, curMovePower, totalMoveCost, 0f);
                            if (curMovePower > 0f)
                            {
                                AStar.MoveableArea(unit, neighborIndex, curMovePower, map, nodeList, enemyList);
                            }
                        }
                    }
                }
            }
        }
    }

    public static void AttackableArea(Character unit, int startIndex, float movePower, 
        GMap map, Dictionary<int, AStarNode> nodeList, List<int> moveRangePath)
    {
        AStarNode astarNode = (!nodeList.ContainsKey(startIndex)) ? null : nodeList[startIndex];
        foreach (int neighborIndex in map.neighborhood(startIndex))
        {
            //如果邻居在移动范围内同时该点没有单位在上面，就不计算
            if (neighborIndex >= 0 && (!moveRangePath.Contains(neighborIndex) || map.GetTileAt(startIndex).character != null))
            {
                if (astarNode == null || neighborIndex != astarNode.prevNodeIndex)
                {
                    if (neighborIndex != unit.tileIndex)
                    {
                        //num2为改变攻击距离格子的入手点，加个判断即可
                        float moveCost = 1;
                        float totalMoveCost = moveCost + ((astarNode != null) ? astarNode.totalCost : 0f);
                        float curMovePower = movePower - moveCost;
                        //在攻击模式就把所有都进行考虑,后面那个不用大于等于，会出bug：寻路可能不全，因为上面的总消耗好像在新节点等于0，就会出现等于的情况
                        if (curMovePower >= 0f && (!nodeList.ContainsKey(neighborIndex) || nodeList[neighborIndex].totalCost >= totalMoveCost))
                        {
                            nodeList[neighborIndex] = new AStarNode(neighborIndex, startIndex, curMovePower, totalMoveCost, 0f);
                            if (curMovePower > 0f)
                            {
                                AStar.AttackableArea(unit, neighborIndex, curMovePower, map, nodeList, moveRangePath);
                            }
                        }
                    }
                }
            }
        }
    }

    public static float MoveCost(Character unit, int startIndex, int endIndex, GMap map, List<int> enemyList)
    {
        if (startIndex == endIndex) return 0f;
        int row = map.getRow(startIndex);
        int col = map.getCol(startIndex);
        int row2 = map.getRow(endIndex);
        int col2 = map.getCol(endIndex);
        if ((row != row2 || Mathf.Abs(col2 - col) != 1) && (col != col2 || Mathf.Abs(row2 - row) != 1))
        {
            return 10000f;
        }

        GMapTile tileAt = map.GetTileAt(startIndex);
        GMapTile tileAt2 = map.GetTileAt(endIndex);
        ////想要跨高度的话，在这里更改，记得在GMapTile里加上能爬的高度
        //if (Mathf.Abs(tileAt2.GetHeight() - tileAt.GetHeight()) > 1)
        //{
        //    return 10000f;//不能爬太高
        //}
        bool isEnemyTile = enemyList.Contains(tileAt2.index);
        if (isEnemyTile)
        {
            return 10000f;
        }

        //int cost = BattleManager.Instance.map.tiles[endIndex].moveCost;
        int cost = map.tiles[endIndex].moveCost;
        //if (actor.occupation == Occupation.Fly && cost < 20)
        //    return 1;   //飞兵无视地形
        //if (cost == -1)
        //{
        //    if (actor.occupation != Occupation.Cavalry) //树林，由于骑兵的原因
        //        return 1;
        //    else
        //        return 3;
        //}
        return cost;
    }
    #endregion

    public static List<int> FindPath(Character unit, int curIndex, int startIndex, int endIndex,
            bool findNearestIfTooFar, float movePower, float stepPower, GMap map, int minRangeInTile,
            int maxRangeInTile, bool canMoveToFriend, bool canMoveToAllFightUnit, List<int> excludeIndexes,
            Dictionary<int, AStarNode> openList, Dictionary<int, AStarNode> closeList, 
            List<int> moveRangePath, List<int> enemyList)
    {
        if (startIndex == endIndex) //如果寻路初始点和目的地相同，返回初始点
            return new List<int> { startIndex };
        if (openList == null)   //如果接下来没有寻路的字典，则创建，初始值为当前点
        {
            openList = new Dictionary<int, AStarNode>();
            closeList = new Dictionary<int, AStarNode>();
            openList[curIndex] = new AStarNode(curIndex, curIndex, movePower, 0f, 0f);
        }
        AStarNode astarNode = openList[curIndex];
        openList.Remove(curIndex);
        closeList[curIndex] = astarNode;    //将当前点放入查找完后的路径
        GMapTile tileAt = map.GetTileAt(curIndex);
        GMapTile tileAt2 = map.GetTileAt(endIndex); //当前点和目标点的格子
        int num = map.Distance(curIndex, endIndex);
        int num2 = tileAt2.GetHeight() - tileAt.GetHeight();
        //在攻击距离内（最短的武器和最长的武器的范围内），或如果可以穿过敌人且当前位置有敌人或者友军当前有友军或者是初始点或没人
        if (num >= minRangeInTile && num <= maxRangeInTile && (tileAt.character == null || tileAt.character == unit))
        {
            List<int> list = new List<int>();
            AStarNode astarNode2 = astarNode;
            while (astarNode2.prevNodeIndex != astarNode2.mapIndex) //将所有点取出，因为只有在初始点才符合
            {
                list.Insert(0, astarNode2.mapIndex);
                astarNode2 = closeList[astarNode2.prevNodeIndex];
            }
            list.Insert(0, astarNode2.mapIndex);    //将初始点插入
            return list;
        }
        if (closeList.Count >= 500)
        {
            return (!findNearestIfTooFar) ? null : AStar.FindPathToNearstEnd(startIndex, endIndex, closeList, map);
        }
        foreach (int neighborIndex in map.neighborhood(curIndex))
        {
            if (neighborIndex >= 0 && moveRangePath.Contains(neighborIndex) && !enemyList.Contains(neighborIndex))
            {
                if (neighborIndex != astarNode.prevNodeIndex)    //如果周围的点没调查过
                {
                    //num4没被排除时获得移动消耗
                    float moveCost = (excludeIndexes == null || !excludeIndexes.Contains(neighborIndex)) ? AStar.MoveCost(unit, curIndex, neighborIndex, map, enemyList) : 10000f;
                    //if (num4 <= stepPower)  //如果消耗小于当前移动力
                    {
                        float num5 = moveCost + astarNode.totalCost;    //总消耗
                        float num6 = movePower - moveCost;  //剩余移动力
                        //if (num6 >= 0f)
                        {
                            if (closeList.ContainsKey(neighborIndex))    //如果找过这个点
                            {
                                if (closeList[neighborIndex].totalCost > num5)   //这个点的总消耗更大（另外一个路径的）
                                {
                                    closeList[neighborIndex] = new AStarNode(neighborIndex, curIndex, num6, num5, astarNode.totalIntentPower + AStar.IntentPower(curIndex, neighborIndex, endIndex, map) + num5 * 3f);
                                }
                            }
                            else if (!openList.ContainsKey(neighborIndex) || openList[neighborIndex].totalCost > num5)
                            {
                                openList[neighborIndex] = new AStarNode(neighborIndex, curIndex, num6, num5, astarNode.totalIntentPower + AStar.IntentPower(curIndex, neighborIndex, endIndex, map) + num5 * 3f);
                            }
                        }
                    }
                }
            }
        }
        List<int> list2 = AStar.SortByPower(openList, map, startIndex, endIndex);
        for (int j = 0; j < list2.Count; j++)
        {
            int num7 = list2[j];
            if (openList.ContainsKey(num7))
            {
                List<int> list3 = AStar.FindPath(unit, num7, startIndex, endIndex, 
                    findNearestIfTooFar, openList[num7].movePowerRemain, stepPower, 
                    map, minRangeInTile, maxRangeInTile, canMoveToFriend, canMoveToAllFightUnit,
                    excludeIndexes, openList, closeList, moveRangePath, enemyList);
                if (list3 != null)
                {
                    return list3;
                }
            }
        }
        return (!findNearestIfTooFar) ? null : AStar.FindPathToNearstEnd(startIndex, endIndex, closeList, map);
    }

    private static List<int> FindPathToNearstEnd(int startIndex, int endIndex, Dictionary<int, AStarNode> closeList, GMap map)
    {
        int num = -1;
        foreach (int num2 in closeList.Keys)
        {
            if ((num < 0 || map.Distance(num2, endIndex) < map.Distance(num, endIndex)) && (num2 == startIndex || !map.GetTileAt(num2).hasActor()))
            {
                num = num2;
            }
        }
        if (num < 0)
        {
            return null;
        }
        List<int> list = new List<int>();
        AStarNode astarNode = closeList[num];
        while (astarNode.prevNodeIndex != astarNode.mapIndex)
        {
            list.Insert(0, astarNode.mapIndex);
            astarNode = closeList[astarNode.prevNodeIndex];
        }
        list.Insert(0, astarNode.mapIndex);
        return list;
    }

    private static List<int> SortByPower(Dictionary<int, AStarNode> openList, GMap map, int startMapIndex, int endMapIndex)
    {
        List<int> list = new List<int>();
        foreach (int num in openList.Keys)
        {
            if (list.Count == 0)
            {
                list.Add(num);
            }
            else
            {
                int num2 = AStar.Bisearch(openList[num], list, openList, map, startMapIndex, endMapIndex, 0, list.Count - 1);
                if (num2 >= list.Count)
                {
                    list.Add(num);
                }
                else
                {
                    list.Insert(num2, num);
                }
            }
        }
        return list;
    }
    //Bisearch是折半查找，找不到应该是返回末尾
    private static int Bisearch(AStarNode node, List<int> nodeMapIndexList, Dictionary<int, AStarNode> openList, GMap map, int startMapIndex, int endMapIndex, int startListIndex, int endListIndex)
    {
        AStarNode node2 = openList[nodeMapIndexList[startListIndex]];
        AStarNode node3 = openList[nodeMapIndexList[endListIndex]];
        if (AStar.CompFunc(node, node2, startMapIndex, endMapIndex, map) <= 0)
        {
            return startListIndex;
        }
        if (AStar.CompFunc(node, node3, startMapIndex, endMapIndex, map) >= 0)
        {
            return endListIndex + 1;
        }
        if (endListIndex - startListIndex < 2)
        {
            return endListIndex;
        }
        int num = (startListIndex + endListIndex) / 2;
        AStarNode node4 = openList[nodeMapIndexList[num]];
        if (AStar.CompFunc(node, node4, startMapIndex, endMapIndex, map) < 0)
        {
            return AStar.Bisearch(node, nodeMapIndexList, openList, map, startMapIndex, endMapIndex, startListIndex, num);
        }
        if (AStar.CompFunc(node, node4, startMapIndex, endMapIndex, map) > 0)
        {
            return AStar.Bisearch(node, nodeMapIndexList, openList, map, startMapIndex, endMapIndex, num, endListIndex);
        }
        return num;
    }
    //这个应该是比较方向的，totalIntentPower越小越好,node1好则为-1，node2好则为1
    private static int CompFunc(AStarNode node1, AStarNode node2, int startIndex, int endIndex, GMap map)
    {
        float totalIntentPower = node1.totalIntentPower;
        float totalIntentPower2 = node2.totalIntentPower;
        if (totalIntentPower < totalIntentPower2)
        {
            return -1;
        }
        if (totalIntentPower > totalIntentPower2)
        {
            return 1;
        }
        float num = AStar.ManhattanPower(node1.mapIndex, endIndex, map);
        float num2 = AStar.ManhattanPower(node2.mapIndex, endIndex, map);
        if (num < num2)
        {
            return -1;
        }
        if (num > num2)
        {
            return 1;
        }
        return 0;
    }
    //应该是用来评估方向的，但是并没有发现在哪使用
    private static float IntentPower(int prevIndex, int curIndex, int destIndex, GMap map)
    {
        int num = 0;
        int row = map.getRow(prevIndex);
        int col = map.getCol(prevIndex);
        int row2 = map.getRow(curIndex);
        int col2 = map.getCol(curIndex);
        int row3 = map.getRow(destIndex);
        int col3 = map.getCol(destIndex);
        if (row3 > row)
        {
            if (row2 > row)
            {
                num--;
            }
            else if (row2 < row)
            {
                num++;
            }
        }
        else if (row3 < row)
        {
            if (row2 < row)
            {
                num--;
            }
            else if (row2 > row)
            {
                num++;
            }
        }
        else if (row2 != row)
        {
            num++;
        }
        if (col3 > col)
        {
            if (col2 > col)
            {
                num--;
            }
            else if (col2 < col)
            {
                num++;
            }
        }
        else if (col3 < col)
        {
            if (col2 < col)
            {
                num--;
            }
            else if (col2 > col)
            {
                num++;
            }
        }
        else if (col2 != col)
        {
            num++;
        }
        float num2 = 0;
        return (float)num + num2;
    }
    //求出曼哈顿距离
    public static float ManhattanPower(int startIndex, int endIndex, GMap map)
    {
        int row = map.getRow(startIndex);
        int col = map.getCol(startIndex);
        int row2 = map.getRow(endIndex);
        int col2 = map.getCol(endIndex);
        return (float)(Mathf.Abs(row2 - row) + Mathf.Abs(col2 - col));
    }
}
