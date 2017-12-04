import { ProductionItem } from './production-item';

export class OrderQuantum {
    public id?: number | {};
    public productionItem?: ProductionItem;
    public orderId?: number;
    public count: number;
    public itemsCountInOnePart: number;

    public machiningFullPartTime?: Date;
    public machiningRemainingFromPartsTime?: Date;

    public assemblingFullBatchTime?: Date;
    public assemblingFullPartTime?: Date;
    public assemblingRemainingFromPartsTime?: Date;

    public machiningStartTimes?: Date[];
    public machiningEndTimes?: Date[];
    public machiningDurations?: Date[];

    public assemblingStartTimes?: Date[];
    public assemblingEndTimes?: Date[];
    public assemblingDurations?: Date[];
}
