public class PlayerXMovementDecisionTree
{
    public enum Cnd
    {
        IS_GROUNDED = 0b000001,
        INPUT_IS_STOP = 0b000010,
        VEL_IS_ZERO = 0b000100,
        INPUT_VEL_SAME_DIRECTION = 0b001000,
        V_OVER_MAX = 0b010000,
        IS_ON_ICE = 0b100000,
    }

    private const int CONDITIONS_MASK = 10;
    private const int CONDITIONS_VALUES = 0;

    public enum NextXVelFunctionIndicator
    {
        STOPPED =
            (((int)Cnd.INPUT_IS_STOP | (int)Cnd.VEL_IS_ZERO) << CONDITIONS_MASK)
            | (((1 * (int)Cnd.INPUT_IS_STOP) | (1 * (int)Cnd.VEL_IS_ZERO)) << CONDITIONS_VALUES),

        STARTUP =
            (
                ((int)Cnd.INPUT_IS_STOP | (int)Cnd.INPUT_VEL_SAME_DIRECTION | (int)Cnd.V_OVER_MAX)
                << CONDITIONS_MASK
            )
            | (
                (
                    (0 * (int)Cnd.INPUT_IS_STOP)
                    | (1 * (int)Cnd.INPUT_VEL_SAME_DIRECTION)
                    | (0 * (int)Cnd.V_OVER_MAX)
                ) << CONDITIONS_VALUES
            ),

        MAXSPEED_GND =
            (
                (
                    (int)Cnd.IS_GROUNDED
                    | (int)Cnd.INPUT_IS_STOP
                    | (int)Cnd.INPUT_VEL_SAME_DIRECTION
                    | (int)Cnd.V_OVER_MAX
                ) << CONDITIONS_MASK
            )
            | (
                (
                    (1 * (int)Cnd.IS_GROUNDED)
                    | (0 * (int)Cnd.INPUT_IS_STOP)
                    | (1 * (int)Cnd.INPUT_VEL_SAME_DIRECTION)
                    | (1 * (int)Cnd.V_OVER_MAX)
                ) << CONDITIONS_VALUES
            ),

        MAXSPEED_AIR =
            (
                (
                    (int)Cnd.IS_GROUNDED
                    | (int)Cnd.INPUT_IS_STOP
                    | (int)Cnd.INPUT_VEL_SAME_DIRECTION
                    | (int)Cnd.V_OVER_MAX
                ) << CONDITIONS_MASK
            )
            | (
                (
                    (0 * (int)Cnd.IS_GROUNDED)
                    | (0 * (int)Cnd.INPUT_IS_STOP)
                    | (1 * (int)Cnd.INPUT_VEL_SAME_DIRECTION)
                    | (1 * (int)Cnd.V_OVER_MAX)
                ) << CONDITIONS_VALUES
            ),

        STOPPING_GND =
            (
                (
                    (int)Cnd.IS_GROUNDED
                    | (int)Cnd.INPUT_IS_STOP
                    | (int)Cnd.VEL_IS_ZERO
                    | (int)Cnd.IS_ON_ICE
                ) << CONDITIONS_MASK
            )
            | (
                (
                    (1 * (int)Cnd.IS_GROUNDED)
                    | (1 * (int)Cnd.INPUT_IS_STOP)
                    | (0 * (int)Cnd.VEL_IS_ZERO)
                    | (0 * (int)Cnd.IS_ON_ICE)
                ) << CONDITIONS_VALUES
            ),

        STOPPING_AIR =
            (
                ((int)Cnd.IS_GROUNDED | (int)Cnd.INPUT_IS_STOP | (int)Cnd.VEL_IS_ZERO)
                << CONDITIONS_MASK
            )
            | (
                (
                    (0 * (int)Cnd.IS_GROUNDED)
                    | (1 * (int)Cnd.INPUT_IS_STOP)
                    | (0 * (int)Cnd.VEL_IS_ZERO)
                ) << CONDITIONS_VALUES
            ),

        TURNING =
            (((int)Cnd.INPUT_IS_STOP | (int)Cnd.INPUT_VEL_SAME_DIRECTION) << CONDITIONS_MASK)
            | (
                ((0 * (int)Cnd.INPUT_IS_STOP) | (0 * (int)Cnd.INPUT_VEL_SAME_DIRECTION))
                << CONDITIONS_VALUES
            ),

        ICE =
            (((int)Cnd.INPUT_IS_STOP | (int)Cnd.IS_ON_ICE) << CONDITIONS_MASK)
            | (((1 * (int)Cnd.INPUT_IS_STOP) | (1 * (int)Cnd.IS_ON_ICE)) << CONDITIONS_VALUES)
    }

    public static bool checkConditionsMatchFunc(int conditions, NextXVelFunctionIndicator indicator)
    {
        return ((conditions ^ (int)indicator) & ((int)indicator >> CONDITIONS_MASK)) == 0b0;
    }
}
