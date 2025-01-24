import {Component, HostListener, Input} from '@angular/core';

@Component({
  selector: 'app-contextmenu',
  standalone: false,

  templateUrl: './contextmenu.component.html',
  styleUrl: './contextmenu.component.css'
})
export class ContextmenuComponent {

  @Input() x: number = 0;
  @Input() y: number = 0;

  @HostListener('document:click', ['$event'])
  clickout(event: any) {
    // Manually added check for outside click to close menu
    this.x = -1;
    this.y = -1;
  }


  contextMenuInfo: any = {
      pageX: 0,
      pageY: 0,
      willContextMenuShow: false
    };

  contextClicked: boolean = false;


  contextMenuOpen = false;

  onContextItem1Click() {
    // Code für Option 1
    this.contextMenuOpen = false;
  }

  onContextItem2Click() {
    // Code für Option 2
    this.contextMenuOpen = false;
  }



  /*
  toggleContextMenu(showContextMenu: boolean, event: MouseEvent | null = null){
    console.log(event);
    if( event !== null) {
      event.preventDefault();
      this.contextMenuInfo.clientX = event.pageX
      this.contextMenuInfo.clientY = event.pageY
    }

    this.contextMenuInfo.willContextMenuShow = showContextMenu;
  }
  */

}
