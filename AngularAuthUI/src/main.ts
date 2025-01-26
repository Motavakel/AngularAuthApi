import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';

platformBrowserDynamic().bootstrapModule(AppModule, {
  ngZoneEventCoalescing: true
})
  .catch(err => console.error(err));



/*import { bootstrapApplication } from '@angular/platform-browser';
  import { AppComponent } from './app/app.component';
  import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

  bootstrapApplication(AppComponent, {
    providers: [
      provideHttpClient(withInterceptorsFromDi())
    ]
  }).catch(err => console.error(err)); */
