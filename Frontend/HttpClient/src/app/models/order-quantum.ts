import { ProductionItem } from './production-item';

export class OrderQuantum {
    public id?: number | {};
    public productionItem?: ProductionItem;
    public orderId?: number;
    public count: number;
    public itemsCountInOnePart: number;

    public machiningFullPartTime?: number;
    public machiningRemainingFromPartsTime?: number;

    public assemblingFullBatchTime?: number;
    public assemblingFullPartTime?: number;
    public assemblingRemainingFromPartsTime?: number;

    public machiningStartTimes?: number[];
    public machiningEndTimes?: number[];
    public machiningDurations?: number[];

    public assemblingStartTimes?: number[];
    public assemblingEndTimes?: number[];
    public assemblingDurations?: number[];
}
