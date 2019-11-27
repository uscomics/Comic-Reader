using System;
using System.Collections.Generic;

[Serializable]
public class IssueCompare : IComparer<Issue> {
    public int Compare(Issue x, Issue y)  { 
        if (x == null || y == null)  {  return 0;  } 
        if (x.id != y.id) { return x.id.CompareTo(y.id); }
        { return x.issue.CompareTo(y.issue); }
    }     
}
