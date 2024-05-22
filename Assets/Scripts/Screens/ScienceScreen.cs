using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceScreen : LMCCScreen
{
    [SerializeField] private GameObject specBlockPrefab;
    [SerializeField] private Transform specBlockParent;

    private List<MIKESpecBlock> specBlocks;

    void Awake()
    {
        specBlocks = new List<MIKESpecBlock>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TSSManager.Main.OnSpecUpdated += UpdateSpec;

        foreach (MIKESpecBlock block in specBlocks)
        {
            block.ExpandedFade.Display(false);
        }
    }

    void OnEnable()
    {
        RemoveAllSpecBlocks();

        foreach (SpecData data in MIKESpecManager.Main.SpecData)
        {
            SpawnSpecBlock(data);
        }
    }

    private void UpdateSpec(SpecData data)
    {
        if (data.id > 0)
        {
            SpawnSpecBlock(data);
        }
    }

    private void SpawnSpecBlock(SpecData data)
    {
        MIKESpecBlock block = Instantiate(specBlockPrefab, specBlockParent).GetComponent<MIKESpecBlock>();
        block.SpecData = data;
        specBlocks.Add(block);
        block.DisplaySpecData();
    }

    public override void ScreenDeactivated()
    {
        RemoveAllSpecBlocks();
    }

    private void RemoveAllSpecBlocks()
    {
        foreach (MIKESpecBlock block in specBlocks)
        {
            Destroy(block.gameObject);
        }
        specBlocks.Clear();
    }
}
