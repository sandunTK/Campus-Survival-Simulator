using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int energy = 70;
    public int grade = 60;
    public int happiness = 65;

    public Slider energyBar;
    public Slider gradeBar;
    public Slider happinessBar;

    void Start()
    {
        UpdateUI();
    }

    public void ChangeEnergy(int value)
    {
        energy += value;
        energy = Mathf.Clamp(energy, 0, 100);
        UpdateUI();
    }

    public void ChangeGrade(int value)
    {
        grade += value;
        grade = Mathf.Clamp(grade, 0, 100);
        UpdateUI();
    }

    public void ChangeHappiness(int value)
    {
        happiness += value;
        happiness = Mathf.Clamp(happiness, 0, 100);
        UpdateUI();
    }

    void UpdateUI()
    {
        energyBar.value = energy;
        gradeBar.value = grade;
        happinessBar.value = happiness;
    }
}