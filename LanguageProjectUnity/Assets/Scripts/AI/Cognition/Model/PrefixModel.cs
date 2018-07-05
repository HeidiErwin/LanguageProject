using System;
using System.Collections.Generic;

// a model which stores sentences
// in a "prefix tree" format, which is
// more space efficient than the simple model
public class PrefixModel : IModel {
    bool hasBlank = false;
    private Dictionary<String, PrefixModel[]> entriesByHead = new Dictionary<String, PrefixModel[]>();

    public bool Contains(Expression e) {
        if (e == null) {
            return hasBlank;
        }

        if (entriesByHead.ContainsKey(e.GetHead())) {
            PrefixModel[] argTrees = entriesByHead[e.GetHead()];
            for (int i = 0; i < argTrees.Length; i++) {
                Expression arg = e.GetArg(i);
                if (!argTrees[i].Contains(arg)) {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public bool Add(Expression e) {
        bool changed = false;
        
        if (e == null) {
            if (hasBlank) {
                return false;    
            }
            hasBlank = true;
            return true;
        }

        String head = e.GetHead();
        int numArgs = e.GetNumArgs();
        if (!entriesByHead.ContainsKey(head)) {

            entriesByHead.Add(head, new PrefixModel[numArgs]);
            changed = true;
        }

        PrefixModel[] argTrees = entriesByHead[head];

        for (int i = 0; i < numArgs; i++) {
            if (argTrees[i] == null) {
                argTrees[i] = new PrefixModel();
            }
            changed = argTrees[i].Add(e.GetArg(i)) || changed;
        }


        return changed;
    }

    private bool IsEmpty<T>(T[] arr) {
        for (int i = 0; i < arr.Length; i++) {
            if (arr[i] != null) {
                return false;
            }
        }
        return true;
    }

    public bool Remove(Expression e) {
        if (e == null) {
            if (hasBlank) {
                hasBlank = false;
                return true;
            }
            return false;
        }

        String head = e.GetHead();
        
        if (!entriesByHead.ContainsKey(head)) {
            return false;
        }

        int numArgs = e.GetNumArgs();
        
        if (numArgs == 0) {
            entriesByHead.Remove(head);
            return true;
        }

        PrefixModel[] argTrees = entriesByHead[head];
        bool shouldDeleteArgs = true;
        for (int i = 0; i < numArgs; i++) {
            if (!argTrees[i].Contains(e.GetArg(i))) {
                shouldDeleteArgs = false;
            }
        }

        bool deletionSuccessful = false;
        if (shouldDeleteArgs) {
            for (int i = 0; i < numArgs; i++) {
                deletionSuccessful = argTrees[i].Remove(e.GetArg(i)) || deletionSuccessful;
            }
            return deletionSuccessful;
        }
        return false;
    }
}
