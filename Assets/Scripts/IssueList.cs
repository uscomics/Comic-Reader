using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IssueList {
    public List<Issue> Issues = new List<Issue>();

    public bool HasIssue(string id, int issue) {
        foreach (Issue book in Issues) {
            if (book.id != id || book.issue != issue) continue;
            return true;
        }
        return false;
    }
    public Issue GetIssue(string id, int issue) {
        foreach (Issue book in Issues) {
            if (book.id != id || book.issue != issue) continue;
            return book;
        }
        return null;
    }
    public void AddIssue(string id, int issue) {
        if (HasIssue(id, issue)) return;
        Issue newIssue = new Issue();
        newIssue.id = id;
        newIssue.issue = issue;
        Issues.Add(newIssue);
    }
    public void RemoveIssue(string id, int issue) {
        foreach (Issue i in Issues) {
            if (i.id != id || i.issue != issue) continue;
            Issues.Remove(i);
            break;
        }
    }
    public string ToJSON() { return  JsonUtility.ToJson(Issues.ToArray()); }
    public void FromJSON(string json) {
        Issue[] objects = JsonHelper.getJsonArray<Issue> (json);
        Issues = new List<Issue>(objects);
    }
    public void SortByKey() {
        IssueCompare issueCompare = new IssueCompare();
        Issues.Sort(issueCompare); 
    }
}
