import { Detail } from './detail';

export class ProductionItemQuantum {
    public id?: number;
    public detail: Detail;
    public productionItemId?: number;
    public count: number;

    public startTimes?: number[];
    public endTimes?: number[];
    public machiningDurations?: number[];
}
