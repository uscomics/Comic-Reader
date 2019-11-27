using System;

[Serializable]
public class Issue {
	public string id = "";
	public int issue = 0;
	public string MakeKey() { return  id + "_" + issue; }
}

