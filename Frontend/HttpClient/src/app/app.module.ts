import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material';
import { RouterModule, Routes } from '@angular/router';

import { RootComponent } from './components/root/root.component';
import { HeaderComponent } from './components/header/header.component';
import { DetailsComponent } from './components/details/details.component';

const appRoutes: Routes = [
    //{ path: '', redirectTo: '/', pathMatch: 'full' },
    { path: 'details', component: DetailsComponent },
    // { path: '**', component: PageNotFoundComponent },
];

@NgModule({
    declarations: [
        RootComponent,
        HeaderComponent,
        DetailsComponent
    ],
    imports: [
        RouterModule.forRoot(appRoutes),
        BrowserModule,
        MatToolbarModule
    ],
    providers: [],
    bootstrap: [RootComponent]
})
export class AppModule { }
