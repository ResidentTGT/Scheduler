import { ProductionItemQuantum } from './production-item-quantum';

export class ProductionItemQuantumsGroup {
    public productionItemQuantums?: ProductionItemQuantum[];
    public workshopSequence?: number[];
    public workshopStartTimes?: number[];
    public workshopEndTimes?: number[];
    public workshopDurations?: number[];
}
