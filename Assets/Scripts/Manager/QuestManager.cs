using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO questData;
        public bool IsStart { get { return questData.isStarted; } set { questData.isStarted = value; } }
        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }
        public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished = value; } }
    }

    public List<QuestTask> tasks = new List<QuestTask>();
    // public List<QuestData_SO> questDatas = new List<QuestData_SO>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public bool HaveQuest(QuestData_SO data)    //通过外部调用，将任务数据传进该函数，判断列表中有无该任务
    {
        if(data != null)
        {
            return tasks.Any(q => q.questData.questName == data.questName);
        }

        return false;
    }

    public QuestTask GetQuestTask(QuestData_SO data)
    {
        /*这里不直接等于数据，是因为克隆出来的任务数据会被稍作修改
        需要直接用任务名判断两任务是否相同*/
        return tasks.Find(q => q.questData.questName == data.questName);
    }

    public void UpdateQuestProgress(string requireName, int count)   //更新所有任务的进度
    {
        foreach (var item in tasks)
        {
            var matchTask = item.questData.questRequires.Find(r => r.name == requireName);
            if(matchTask != null)
            {
                matchTask.currentAmount += count;
            }

            item.questData.CheckQuestProgress();
        }
    }
}
