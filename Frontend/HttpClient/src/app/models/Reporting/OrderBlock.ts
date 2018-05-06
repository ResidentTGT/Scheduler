import { GroupBlock } from './GroupBlock';

export class OrderBlock {
    public productionItemId: number;

    public productionItemsName: string;

    public productionItemsCount: number;

    public startTime: number;

    public duration: number;

    public isMachining: boolean;

    public groupBlocks: GroupBlock[];
}
