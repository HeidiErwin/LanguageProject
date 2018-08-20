// ALL PREDICATES
// m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.KING, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.RED, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.XXX, Expression.YYY)));

public class Models {
    public static IModel BobModel() {
        IModel m = new PrefixModel();

        // things Bob takes to be true of Bob
        m.Add(new Phrase(Expression.KING, Expression.BOB));
        m.Add(new Phrase(Expression.ACTIVE, Expression.BOB));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, Expression.BOB));
        m.Add(new Phrase(Expression.IN_RED_AREA, Expression.BOB));

        // things Bob takes to be true of Evan
        m.Add(new Phrase(Expression.ACTIVE, Expression.EVAN));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, Expression.EVAN));

        // things Bob takes to be true of no fountain
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.FOUNTAIN)));

        // things Bob takes to be true of a fountain
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.FOUNTAIN)));

        // things Bob takes to be true of two fountains
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.TWO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.TWO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.TWO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.TWO, Expression.FOUNTAIN)));

        // things Bob takes to be true of three fountains
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.THREE, Expression.FOUNTAIN)));

        // things Bob takes to be true of every fountain
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.EVERY, Expression.FOUNTAIN)));

        // things Bob takes to be true of no lamp
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.LAMP)));

        // things Bob takes to be true of a lamp
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.LAMP)));

        // things Bob takes to be true of two lamps
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.TWO, Expression.LAMP)));

        // things Bob takes to be true of three lamps

        // things Bob takes to be true of every lamp
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.EVERY, Expression.LAMP)));

        // things Bob takes to be true of no active thing
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.ACTIVE)));

        // things Bob takes to be true of an active thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.ACTIVE)));

        // things Bob takes to be true of two active things
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.TWO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.TWO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.TWO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.TWO, Expression.ACTIVE)));

        // things Bob takes to be true of three active things
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.THREE, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.THREE, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.THREE, Expression.ACTIVE)));

        // things Bob takes to be true of every active thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.ACTIVE)));

        // things Bob takes to be true of no inactive thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.INACTIVE)));

        // things Bob takes to be true of an inactive thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.INACTIVE)));

        // things Bob takes to be true of two inactive things
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.TWO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.TWO, Expression.INACTIVE)));

        // things Bob takes to be true of three inactive things

        // things Bob takes to be true of every inactive thing
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.EVERY, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.EVERY, Expression.INACTIVE)));

        // things Bob takes to be true of no king
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.KING)));

        // things Bob takes to be true of a king
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.KING)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.A, Expression.KING)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.KING)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.KING)));

        // things Bob takes to be true of two kings
        
        // things Bob takes to be true of three kings
        
        // things Bob takes to be true of every king
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.KING)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.EVERY, Expression.KING)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.EVERY, Expression.KING)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.EVERY, Expression.KING)));

        // things Bob takes to be true of no yellow thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.YELLOW)));

        // things Bob takes to be true of a yellow thing
        
        // things Bob takes to be true of two yellow things
        
        // things Bob takes to be true of three yellow things
        
        // things Bob takes to be true of every yellow thing
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.EVERY, Expression.YELLOW)));

        // things Bob takes to be true of no green thing
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.GREEN)));

        // things Bob takes to be true of a green thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.GREEN)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.GREEN)));
        
        // things Bob takes to be true of two green things
        
        // things Bob takes to be true of three green things
        
        // things Bob takes to be true of every green thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.EVERY, Expression.GREEN)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.EVERY, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.EVERY, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.EVERY, Expression.GREEN)));

        // things Bob takes to be true of no blue thing
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.BLUE)));

        // things Bob takes to be true of a blue thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.BLUE)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.BLUE)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.BLUE)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.A, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.BLUE)));

        // things Bob takes to be true of two blue things
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.TWO, Expression.BLUE)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.TWO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.TWO, Expression.BLUE)));

        // things Bob takes to be true of three blue things
        
        // things Bob takes to be true of every blue thing
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.EVERY, Expression.BLUE)));

        // things Bob takes to be true of no red thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.RED)));

        // things Bob takes to be true of a red thing
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.RED)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.RED)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.A, Expression.RED)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.RED)));

        // things Bob takes to be true of two red things
        
        // things Bob takes to be true of three red things
        
        // things Bob takes to be true of every red thing
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.EVERY, Expression.RED)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.EVERY, Expression.RED)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.EVERY, Expression.RED)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.EVERY, Expression.RED)));

        // things Bob takes to be true of no in-Bob's-area thing
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));

        // things Bob takes to be true of an in-Bob's-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));

        // things Bob takes to be true of two in-Bob's-area things
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.TWO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.TWO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.TWO, Expression.IN_YOUR_AREA)));

        // things Bob takes to be true of three in-Bob's-area things
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.THREE, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.THREE, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.THREE, Expression.IN_YOUR_AREA)));
        
        // things Bob takes to be true of every in-Bob's-area thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.EVERY, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.EVERY, Expression.IN_YOUR_AREA)));

        // things Bob takes to be true of no in-yellow-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));

        // things Bob takes to be true of an in-yellow-area thing

        // things Bob takes to be true of two in-yellow-area things

        // things Bob takes to be true of three in-yellow-area things

        // things Bob takes to be true of every in-yellow-area thing
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.EVERY, Expression.IN_YELLOW_AREA)));

        // things Bob takes to be true of no in-green-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));

        // things Bob takes to be true of an in-green-area thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.A, Expression.IN_GREEN_AREA)));

        // things Bob takes to be true of two in-green-area things

        // things Bob takes to be true of three in-green-area things

        // things Bob takes to be true of every in-green-area thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.EVERY, Expression.IN_GREEN_AREA)));

        // things Bob takes to be true of no in-blue-area thing
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));

        // things Bob takes to be true of an in-blue-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));

        // things Bob takes to be true of two in-blue-area things
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.TWO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.TWO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.TWO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.TWO, Expression.IN_BLUE_AREA)));

        // things Bob takes to be true of three in-blue-area things
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.THREE, Expression.IN_BLUE_AREA)));

        // things Bob takes to be true of every in-blue-area thing
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.EVERY, Expression.IN_BLUE_AREA)));

        // things Bob takes to be true of no in-red-area thing
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.IN_RED_AREA)));

        // things Bob takes to be true of an in-red-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.IN_RED_AREA)));

        // things Bob takes to be true of two in-red-area things
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.TWO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.TWO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.TWO, Expression.IN_RED_AREA)));

        // things Bob takes to be true of three in-red-area things
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.THREE, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.THREE, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.THREE, Expression.IN_RED_AREA)));
        
        // things Bob takes to be true of every in-red-area thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.EVERY, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.EVERY, Expression.IN_RED_AREA)));

        return m;
    }

    public static IModel EvanModel() {
        IModel m = new PrefixModel();

        // things Evan takes to be true of Bob
        m.Add(new Phrase(Expression.ACTIVE, Expression.BOB));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, Expression.BOB));
        m.Add(new Phrase(Expression.IN_RED_AREA, Expression.BOB));

        // things Evan takes to be true of Evan
        m.Add(new Phrase(Expression.KING, Expression.EVAN));
        m.Add(new Phrase(Expression.ACTIVE, Expression.EVAN));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, Expression.EVAN));

        // things Evan takes to be true of no fountain
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.FOUNTAIN)));

        // things Evan takes to be true of a fountain
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.FOUNTAIN)));

        // things Evan takes to be true of two fountains
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.TWO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.TWO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.TWO, Expression.FOUNTAIN)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.TWO, Expression.FOUNTAIN)));

        // things Evan takes to be true of three fountains
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.THREE, Expression.FOUNTAIN)));

        // things Evan takes to be true of every fountain
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.EVERY, Expression.FOUNTAIN)));

        // things Evan takes to be true of no lamp
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.LAMP)));

        // things Evan takes to be true of a lamp
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.LAMP)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.LAMP)));

        // things Evan takes to be true of two lamps
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.TWO, Expression.LAMP)));

        // things Evan takes to be true of three lamps

        // things Evan takes to be true of every lamp
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.EVERY, Expression.LAMP)));

        // things Evan takes to be true of no active thing
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.ACTIVE)));

        // things Evan takes to be true of an active thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.ACTIVE)));

        // things Evan takes to be true of two active things
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.TWO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.TWO, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.TWO, Expression.ACTIVE)));

        // things Evan takes to be true of three active things
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.THREE, Expression.ACTIVE)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.THREE, Expression.ACTIVE)));

        // things Evan takes to be true of every active thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.ACTIVE)));

        // things Evan takes to be true of no inactive thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.INACTIVE)));

        // things Evan takes to be true of an inactive thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.INACTIVE)));

        // things Evan takes to be true of two inactive things
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.TWO, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.TWO, Expression.INACTIVE)));

        // things Evan takes to be true of three inactive things

        // things Evan takes to be true of every inactive thing
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.EVERY, Expression.INACTIVE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.EVERY, Expression.INACTIVE)));

        // things Evan takes to be true of no king
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.KING)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.KING)));

        // things Evan takes to be true of a king
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.KING)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.A, Expression.KING)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.KING)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.A, Expression.KING)));

        // things Evan takes to be true of two kings
        
        // things Evan takes to be true of three kings
        
        // things Evan takes to be true of every king
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.KING)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.EVERY, Expression.KING)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.EVERY, Expression.KING)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.EVERY, Expression.KING)));

        // things Evan takes to be true of no yellow thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.YELLOW)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.YELLOW)));

        // things Evan takes to be true of a yellow thing
        
        // things Evan takes to be true of two yellow things
        
        // things Evan takes to be true of three yellow things
        
        // things Evan takes to be true of every yellow thing
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.EVERY, Expression.YELLOW)));

        // things Evan takes to be true of no green thing
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.GREEN)));

        // things Evan takes to be true of a green thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.GREEN)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.GREEN)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.GREEN)));
        
        // things Evan takes to be true of two green things
        
        // things Evan takes to be true of three green things
        
        // things Evan takes to be true of every green thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.EVERY, Expression.GREEN)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.EVERY, Expression.GREEN)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.EVERY, Expression.GREEN)));

        // things Evan takes to be true of no blue thing
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.BLUE)));

        // things Evan takes to be true of a blue thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.BLUE)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.BLUE)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.BLUE)));

        // things Evan takes to be true of two blue things
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.TWO, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.TWO, Expression.BLUE)));

        // things Evan takes to be true of three blue things
        
        // things Evan takes to be true of every blue thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.EVERY, Expression.BLUE)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.EVERY, Expression.BLUE)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.EVERY, Expression.BLUE)));

        // things Evan takes to be true of no red thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.RED)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.RED)));

        // things Evan takes to be true of a red thing
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.RED)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.RED)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.A, Expression.RED)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.RED)));
        
        // things Evan takes to be true of two red things
        
        // things Evan takes to be true of three red things
        
        // things Evan takes to be true of every red thing
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.EVERY, Expression.RED)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.EVERY, Expression.RED)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.EVERY, Expression.RED)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.EVERY, Expression.RED)));

        // things Evan takes to be true of no in-Evan's-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.IN_YOUR_AREA)));

        // things Evan takes to be true of an in-Evan's-area thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.A, Expression.IN_YOUR_AREA)));

        // things Evan takes to be true of two in-Evan's-area things
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.TWO, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.TWO, Expression.IN_YOUR_AREA)));

        // things Evan takes to be true of three in-Evan's-area things
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.THREE, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.THREE, Expression.IN_YOUR_AREA)));
        
        // things Evan takes to be true of every in-Evan's-area thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.EVERY, Expression.IN_YOUR_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.EVERY, Expression.IN_YOUR_AREA)));

        // things Evan takes to be true of no in-yellow-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.IN_YELLOW_AREA)));

        // things Evan takes to be true of an in-yellow-area thing

        // things Evan takes to be true of two in-yellow-area things

        // things Evan takes to be true of three in-yellow-area things

        // things Evan takes to be true of every in-yellow-area thing
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.EVERY, Expression.IN_YELLOW_AREA)));

        // things Evan takes to be true of no in-green-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.IN_GREEN_AREA)));

        // things Evan takes to be true of an in-green-area thing
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.A, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.A, Expression.IN_GREEN_AREA)));

        // things Evan takes to be true of two in-green-area things

        // things Evan takes to be true of three in-green-area things

        // things Evan takes to be true of every in-green-area thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.IN_GREEN_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.EVERY, Expression.IN_GREEN_AREA)));

        // things Evan takes to be true of no in-blue-area thing
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.NO, Expression.IN_BLUE_AREA)));

        // things Evan takes to be true of an in-blue-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.A, Expression.IN_BLUE_AREA)));

        // things Evan takes to be true of two in-blue-area things
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.TWO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.TWO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.TWO, Expression.IN_BLUE_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.TWO, Expression.IN_BLUE_AREA)));

        // things Evan takes to be true of three in-blue-area things
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.THREE, Expression.IN_BLUE_AREA)));

        // things Evan takes to be true of every in-blue-area thing
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.EVERY, Expression.IN_BLUE_AREA)));

        // things Evan takes to be true of no in-red-area thing
        m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.KING, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.RED, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.NO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.NO, Expression.IN_RED_AREA)));

        // things Evan takes to be true of an in-red-area thing
        m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.A, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.A, Expression.IN_RED_AREA)));

        // things Evan takes to be true of two in-red-area things
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.TWO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.TWO, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.TWO, Expression.IN_RED_AREA)));

        // things Evan takes to be true of three in-red-area things
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.THREE, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.THREE, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.THREE, Expression.IN_RED_AREA)));
        
        // things Evan takes to be true of every in-red-area thing
        m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.EVERY, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.EVERY, Expression.IN_RED_AREA)));
        m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.EVERY, Expression.IN_RED_AREA)));

        return m;
    }
}
