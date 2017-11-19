import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
    MatButtonModule, MatToolbarModule, MatIconModule, MatPaginatorModule, MatListModule, MatProgressSpinnerModule, MatSlideToggleModule,
    MatGridListModule, MatDatepickerModule, MatFormFieldModule, MatNativeDateModule, MatInputModule, MatSliderModule, MatSliderChange,
    MatSelectModule, MatCardModule, MatDialogModule
} from '@angular/material';
import { RouterModule, Routes } from '@angular/router';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { RootComponent } from './components/root/root.component';
import { HeaderComponent } from './components/header/header.component';
import { DetailsComponent } from './components/details/details.component';
import { BackendApiService } from './services/backend-api.service';
import { EquipmentsComponent } from './components/equipments/equipments.component';
import { EquipmentTypePipe } from './pipes/equipment-type.pipe';
import { OrdersComponent } from './components/orders/orders.component';
import { ProductionItemsComponent } from './components/production-items/production-items.component';
import { CreateProductionItemComponent } from './components/create-production-item/create-production-item.component';

const appRoutes: Routes = [
    { path: '', redirectTo: '/', pathMatch: 'full' },
    { path: 'details', component: DetailsComponent },
    { path: 'equipment', component: EquipmentsComponent },
    { path: 'orders', component: OrdersComponent },
    { path: 'production-items', component: ProductionItemsComponent },
    // { path: '**', component: PageNotFoundComponent },
];

@NgModule({
    declarations: [
        RootComponent,
        HeaderComponent,
        DetailsComponent,
        EquipmentsComponent,
        EquipmentTypePipe,
        OrdersComponent,
        ProductionItemsComponent,
        CreateProductionItemComponent
    ],
    imports: [
        RouterModule.forRoot(appRoutes),
        BrowserModule,
        HttpModule,
        MatButtonModule,
        MatToolbarModule,
        MatIconModule,
        MatPaginatorModule,
        BrowserAnimationsModule,
        MatListModule,
        MatProgressSpinnerModule,
        MatGridListModule,
        MatDatepickerModule,
        MatFormFieldModule,
        MatNativeDateModule,
        MatInputModule,
        MatSliderModule,
        MatSlideToggleModule,
        MatSelectModule,
        FormsModule,
        MatCardModule,
        MatDialogModule
    ],
    providers: [BackendApiService],
    bootstrap: [RootComponent],
    entryComponents: [CreateProductionItemComponent]
})
export class AppModule { }
