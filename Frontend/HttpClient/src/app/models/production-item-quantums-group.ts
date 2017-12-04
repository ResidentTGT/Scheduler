import { ProductionItemQuantum } from "./production-item-quantum";

export class ProductionItemQuantumsGroup {
    public productionItemQuantums?: ProductionItemQuantum[];
    public workshopSequence?: number[];
    public workshopStartTimes?: Date[];
    public workshopEndTimes?: Date[];
    public workshopDurations?: Date[];
}
