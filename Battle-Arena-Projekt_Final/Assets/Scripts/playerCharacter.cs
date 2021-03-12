using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class playerCharacter : MonoBehaviour
{
    
    public static string name;
  

    public static void setName( string _name)
    {
        name = _name;
    }

    public static string getName()
    {

        return name;
    }
}
