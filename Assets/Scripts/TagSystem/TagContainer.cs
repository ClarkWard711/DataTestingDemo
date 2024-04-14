using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagContainer : MonoBehaviour
{
    public LinkedList<TagInfo> TagList = new LinkedList<TagInfo>();

    public void AddTag(TagInfo tagInfo)
    {
        TagInfo findTagInfo = FindTag(tagInfo.tag.ID);

        if (findTagInfo != null) 
        {
            //tag存在
            switch (findTagInfo.tag.TagKind)
            {
                case Tag.Kind.accumulable:
                    findTagInfo.stack++;
                    break;

                case Tag.Kind.turnLessen:
                    findTagInfo.turnDuration += findTagInfo.tag.TurnAdd;
                    break;

                case Tag.Kind.eternal:
                    break;
            }
            findTagInfo.tag.OnCreate.Apply(findTagInfo);
        }
        else
        {
            //tag不存在
        }
    }

    private TagInfo FindTag(int ID)
    {
        foreach (var tagInfo in TagList)
        {
            if (tagInfo.tag.ID == ID) 
            {
                return tagInfo;
            }
        }

        return default;
    }
}
