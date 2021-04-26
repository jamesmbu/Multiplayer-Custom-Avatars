using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Reflection;

public class CharacterCustomisation : MonoBehaviourPun
{
    public enum MODEL_DETAILS
    {
        HAIR_MODEL,
        BEARD_MODEL,
        OUTFIT_MODEL,
        PROP_MODEL,
        SKIN_COLOUR,
        HAIR_COLOUR,

    }
    public GameObject Canvas;
    public GameObject PlayerHead;
    public GameObject PlayerBody;

    public GameObject[] Hair;
    public GameObject[] Beard;
    public GameObject[] Outfit;
    public GameObject[] Prop;

    public Texture[] skinTexture;
    public Texture[] beardTextures;

    // Hair Textures 
    public Texture[] hair_a_textures;
    public Texture[] hair_b_textures;
    public Texture[] hair_c_textures;
    public Texture[] hair_d_textures;
    public Texture[] hair_e_textures;

    //hair
    public GameObject currentHair;
    private GameObject PrevHair;
    //beard
    public GameObject currentBeard;
    private GameObject PrevBeard;
    //outfit
    public GameObject currentOutfit;
    private GameObject PrevOutfit;
    //prop
    public GameObject CurrentProps;
    private GameObject PrevProps;

    private Texture currentHairColour;
    private Texture PrevHairColour;
    private Texture PrevSkin;
    private Texture currentskin;


    int HairIndex = 0;
    int BeardIndex = 0;
    int OutfitIndex = 0;
    int PropIndex = 0;
    int skinTextureIndex = 0;
    int BeardTexture = 0;

    public Dictionary<MODEL_DETAILS, int> Save_Model;

    bool IndexCheck(int index, GameObject[] arraylength)
    {
        int length = arraylength.Length;
        return index <(length-1);
    }
    public void ModifyHairUp()
    {
        if(IndexCheck(HairIndex, Hair))
            HairIndex++; 
        else
            HairIndex = 0;
        ApplyModification(MODEL_DETAILS.HAIR_MODEL, HairIndex);
    }

    public void ModifyHairDown()
    {
        if (HairIndex < Hair.Length && HairIndex != 0) 
            HairIndex--;
        else if (HairIndex == 0)
            HairIndex = Hair.Length - 1;
        else 
            HairIndex = 0; 
        ApplyModification(MODEL_DETAILS.HAIR_MODEL, HairIndex);
    }

    public void ModifyBeardUp()
    {
        if (IndexCheck(BeardIndex, Beard))
            BeardIndex++;
        else
            BeardIndex = 0;
        ApplyModification(MODEL_DETAILS.BEARD_MODEL, BeardIndex);
    }

    public void ModifyBeardDown()
    {
        if (BeardIndex < Beard.Length && BeardIndex != 0)
            BeardIndex--;
        else if (BeardIndex == 0)
            BeardIndex = Beard.Length-1;
        else
            BeardIndex = 0;
        Debug.Log(BeardIndex);
        ApplyModification(MODEL_DETAILS.BEARD_MODEL, BeardIndex);
    }


    public void ModifyOutfitUp()
    {
        if (IndexCheck(OutfitIndex, Outfit))
            OutfitIndex++;
        else
            OutfitIndex = 0;
        ApplyModification(MODEL_DETAILS.OUTFIT_MODEL, OutfitIndex);
    }

    public void ModifyOutfitDown()
    {
        if (OutfitIndex < Outfit.Length && OutfitIndex != 0)
            OutfitIndex--;
        else if (OutfitIndex == 0)
            OutfitIndex = Outfit.Length - 1;
        else
            OutfitIndex = 0;
        ApplyModification(MODEL_DETAILS.OUTFIT_MODEL, OutfitIndex);
    }

    public void ModifyPropUp()
    {
        if (IndexCheck(PropIndex, Prop))
            PropIndex++;
        else
            PropIndex = 0;
        ApplyModification(MODEL_DETAILS.PROP_MODEL, PropIndex);
    }

    public void ModifyPropDown()
    {
        if (PropIndex < Prop.Length && PropIndex != 0)
            PropIndex--;
        else if (PropIndex == 0)
            PropIndex = Prop.Length - 1;
        else
            PropIndex = 0;
        ApplyModification(MODEL_DETAILS.PROP_MODEL, PropIndex);
    }

    public void ApplySavedAppearance(Dictionary<MODEL_DETAILS, int> _Save_Model) // For initialising appearance; for use if customisation has already happened
    {
        if (photonView.IsMine)
        {
            Save_Model = _Save_Model;
            /*ApplyModification(MODEL_DETAILS.HAIR_MODEL, _Save_Model[MODEL_DETAILS.HAIR_MODEL]);
            ApplyModification(MODEL_DETAILS.BEARD_MODEL, _Save_Model[MODEL_DETAILS.BEARD_MODEL]);
            ApplyModification(MODEL_DETAILS.OUTFIT_MODEL, _Save_Model[MODEL_DETAILS.OUTFIT_MODEL]);
            ApplyModification(MODEL_DETAILS.PROP_MODEL, _Save_Model[MODEL_DETAILS.PROP_MODEL]);*/
            
            // Broadcast appearance changes across all remote players (and the local instance)
            photonView.RPC("ApplyModification", RpcTarget.AllBuffered,
                MODEL_DETAILS.HAIR_MODEL, _Save_Model[MODEL_DETAILS.HAIR_MODEL]);

            photonView.RPC("ApplyModification", RpcTarget.AllBuffered,
                MODEL_DETAILS.BEARD_MODEL, _Save_Model[MODEL_DETAILS.BEARD_MODEL]);
            
            photonView.RPC("ApplyModification", RpcTarget.AllBuffered,
                            MODEL_DETAILS.OUTFIT_MODEL, _Save_Model[MODEL_DETAILS.OUTFIT_MODEL]);
           
            photonView.RPC("ApplyModification", RpcTarget.AllBuffered,
                MODEL_DETAILS.PROP_MODEL, _Save_Model[MODEL_DETAILS.PROP_MODEL]);

            photonView.RPC(nameof(apply), RpcTarget.AllBuffered);
        }
        
    }

    
    [PunRPC] void ApplyModification(MODEL_DETAILS details, int id)
    {
        
            
            switch (details)
            {
                case MODEL_DETAILS.HAIR_MODEL:
                    if (PrevHair != null)
                    {
                        PrevHair.SetActive(false);
                    }
                    currentHair = Hair[id];
                    currentHair.SetActive(true);
                    PrevHair = currentHair;
                    break;
                case MODEL_DETAILS.BEARD_MODEL:
                    if (PrevBeard != null)
                    {
                        PrevBeard.SetActive(false);
                    }
                    currentBeard = Beard[id];
                    currentBeard.SetActive(true);
                    PrevBeard = currentBeard;
                    break;
                case MODEL_DETAILS.OUTFIT_MODEL:
                    if (PrevOutfit != null)
                    {
                        PrevOutfit.SetActive(false);
                    }
                    currentOutfit = Outfit[id];
                    currentOutfit.SetActive(true);
                    PrevOutfit = currentOutfit;
                    break;
                case MODEL_DETAILS.PROP_MODEL:
                    if (PrevProps != null)
                    {
                        PrevProps.SetActive(false);
                    }
                    CurrentProps = Prop[id];
                    CurrentProps.SetActive(true);
                    PrevProps = CurrentProps;
                    break;

                default: break;
            }
        
    }

    public void SaveButton()
    {
        Save_Model = new Dictionary<MODEL_DETAILS, int>();
        Save_Model.Add(MODEL_DETAILS.HAIR_MODEL, HairIndex);
        Save_Model.Add(MODEL_DETAILS.BEARD_MODEL, BeardIndex);
        Save_Model.Add(MODEL_DETAILS.OUTFIT_MODEL, OutfitIndex);
        Save_Model.Add(MODEL_DETAILS.PROP_MODEL, PropIndex);
        //GameObject.Destroy(Canvas);
        // Canvas will be hidden in the NetworkManager 'OnJoinedRoom'
    }

    [PunRPC]
    public void apply()
    {
        currentHair.SetActive(true);
        currentBeard.SetActive(true);
        currentOutfit.SetActive(true);
        CurrentProps = Prop[0];
        CurrentProps.SetActive(true);
    }
}
