

namespace Mini_aventyr;
public class Health {
    public float MaxHP { get; private set; }
    public float HP { get; private set; }

    public bool IsDead => HP <= 0;
    public Health (float maxHp, float hp) {
        MaxHP = maxHp;
        HP = hp;
    }
    public void Rest () {
        HP++; 
        if(HP >= MaxHP)  HP = MaxHP; 
    }

    public void Damage(float damage) {
        HP -= damage;
    }

}

