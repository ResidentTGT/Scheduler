import { Detail } from './detail';

export class ProductionItemQuantum {
    public id?: number;
    public detail: Detail;
    public productionItemId?: number;
    public count: number;

    public startTimes?: Date[];
    public endTimes?: Date[];
    public machiningDurations?: Date[];
}
