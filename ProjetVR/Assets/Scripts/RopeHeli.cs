using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RopeHeli : Rope
{
    [SerializeField] bool mEndTriggered = false;
    [SerializeField] Transform mHelicopter = null;
    [SerializeField] float mSpeedHeli = 1;

    [SerializeField] Image mBlackscreen = null;
    [SerializeField] float mSpeedBlackScreen = 1;

    public override void UseGPE(Player _playerAttached, Hand _handAttached)
    {
        base.UseGPE(_playerAttached, _handAttached);
        mEndTriggered = true;
    }

    protected override void Update()
    {
        base.Update();
        if (!mEndTriggered) return;
        mHelicopter.transform.position += new Vector3(0,mSpeedHeli * Time.deltaTime,0);
        mBlackscreen.color += new Color(0, 0, 0, mSpeedBlackScreen * Time.deltaTime);

        if (mBlackscreen.color.a >= 1) SceneManager.LoadScene("MainMenu");
    }
}
