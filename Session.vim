let SessionLoad = 1
if &cp | set nocp | endif
let s:cpo_save=&cpo
set cpo&vim
inoremap <silent> <expr> <Plug>(asyncomplete_force_refresh) asyncomplete#force_refresh()
inoremap <silent> <Plug>(ale_complete) :ALEComplete
noremap <silent>  :TmuxNavigateLeft
noremap <silent> <NL> :TmuxNavigateDown
noremap <silent>  :TmuxNavigateUp
noremap <silent>  :TmuxNavigateRight
map  :NERDTreeToggle
noremap <silent>  :TmuxNavigatePrevious
nmap \<F8> <Plug>VimspectorRunToCursor
nmap \<F9> <Plug>VimspectorToggleConditionalBreakpoint
nmap \<F5> <Plug>VimspectorLaunch
map \l :VroomRunLastTest
map \R :VroomRunNearestTest
map \r :VroomRunTestFile
xmap gx <Plug>NetrwBrowseXVis
nmap gx <Plug>NetrwBrowseX
xnoremap <silent> <Plug>NetrwBrowseXVis :call netrw#BrowseXVis()
nnoremap <silent> <Plug>NetrwBrowseX :call netrw#BrowseX(netrw#GX(),netrw#CheckIfRemote(netrw#GX()))
nmap <F12> <Plug>VimspectorStepOut
nmap <F11> <Plug>VimspectorStepInto
nmap <F10> <Plug>VimspectorStepOver
nmap <F8> <Plug>VimspectorAddFunctionBreakpoint
nmap <F9> <Plug>VimspectorToggleBreakpoint
nmap <F6> <Plug>VimspectorPause
nmap <F4> <Plug>VimspectorRestart
nmap <F3> <Plug>VimspectorStop
nmap <F5> <Plug>VimspectorContinue
nnoremap <silent> <Plug>VimspectorDisassemble :call vimspector#ShowDisassembly()
nnoremap <silent> <Plug>VimspectorBreakpoints :call vimspector#ListBreakpoints()
nnoremap <silent> <Plug>VimspectorJumpToProgramCounter :call vimspector#JumpToProgramCounter()
nnoremap <silent> <Plug>VimspectorPause :call vimspector#Pause()
nnoremap <silent> <Plug>VimspectorRestart :call vimspector#Restart()
nnoremap <silent> <Plug>VimspectorStop :call vimspector#Stop()
nnoremap <silent> <Plug>VimspectorLaunch :call vimspector#Launch( v:true )
noremap <silent> <C-Bslash> :TmuxNavigatePrevious
noremap <silent> <C-L> :TmuxNavigateRight
noremap <silent> <C-K> :TmuxNavigateUp
noremap <silent> <C-J> :TmuxNavigateDown
noremap <silent> <C-H> :TmuxNavigateLeft
nnoremap <silent> <Plug>(CommandTHistory) :CommandTHistory
nnoremap <silent> <Plug>(CommandTTag) :CommandTTag
nnoremap <silent> <Plug>(CommandTSearch) :CommandTSearch
nnoremap <silent> <Plug>(CommandTMRU) :CommandTMRU
nnoremap <silent> <Plug>(CommandTLine) :CommandTLine
nnoremap <silent> <Plug>(CommandTJump) :CommandTJump
nnoremap <silent> <Plug>(CommandTCommand) :CommandTCommand
nnoremap <silent> <Plug>(ale_repeat_selection) :ALERepeatSelection
nnoremap <silent> <Plug>(ale_code_action) :ALECodeAction
nnoremap <silent> <Plug>(ale_filerename) :ALEFileRename
nnoremap <silent> <Plug>(ale_rename) :ALERename
nnoremap <silent> <Plug>(ale_import) :ALEImport
nnoremap <silent> <Plug>(ale_documentation) :ALEDocumentation
nnoremap <silent> <Plug>(ale_hover) :ALEHover
nnoremap <silent> <Plug>(ale_find_references) :ALEFindReferences
nnoremap <silent> <Plug>(ale_go_to_implementation_in_vsplit) :ALEGoToImplementation -vsplit
nnoremap <silent> <Plug>(ale_go_to_implementation_in_split) :ALEGoToImplementation -split
nnoremap <silent> <Plug>(ale_go_to_implementation_in_tab) :ALEGoToImplementation -tab
nnoremap <silent> <Plug>(ale_go_to_implementation) :ALEGoToImplementation
nnoremap <silent> <Plug>(ale_go_to_type_definition_in_vsplit) :ALEGoToTypeDefinition -vsplit
nnoremap <silent> <Plug>(ale_go_to_type_definition_in_split) :ALEGoToTypeDefinition -split
nnoremap <silent> <Plug>(ale_go_to_type_definition_in_tab) :ALEGoToTypeDefinition -tab
nnoremap <silent> <Plug>(ale_go_to_type_definition) :ALEGoToTypeDefinition
nnoremap <silent> <Plug>(ale_go_to_definition_in_vsplit) :ALEGoToDefinition -vsplit
nnoremap <silent> <Plug>(ale_go_to_definition_in_split) :ALEGoToDefinition -split
nnoremap <silent> <Plug>(ale_go_to_definition_in_tab) :ALEGoToDefinition -tab
nnoremap <silent> <Plug>(ale_go_to_definition) :ALEGoToDefinition
nnoremap <silent> <Plug>(ale_fix) :ALEFix
nnoremap <silent> <Plug>(ale_detail) :ALEDetail
nnoremap <silent> <Plug>(ale_lint) :ALELint
nnoremap <silent> <Plug>(ale_reset_buffer) :ALEResetBuffer
nnoremap <silent> <Plug>(ale_disable_buffer) :ALEDisableBuffer
nnoremap <silent> <Plug>(ale_enable_buffer) :ALEEnableBuffer
nnoremap <silent> <Plug>(ale_toggle_buffer) :ALEToggleBuffer
nnoremap <silent> <Plug>(ale_reset) :ALEReset
nnoremap <silent> <Plug>(ale_disable) :ALEDisable
nnoremap <silent> <Plug>(ale_enable) :ALEEnable
nnoremap <silent> <Plug>(ale_toggle) :ALEToggle
nnoremap <silent> <Plug>(ale_last) :ALELast
nnoremap <silent> <Plug>(ale_first) :ALEFirst
nnoremap <silent> <Plug>(ale_next_wrap_warning) :ALENext -wrap -warning
nnoremap <silent> <Plug>(ale_next_warning) :ALENext -warning
nnoremap <silent> <Plug>(ale_next_wrap_error) :ALENext -wrap -error
nnoremap <silent> <Plug>(ale_next_error) :ALENext -error
nnoremap <silent> <Plug>(ale_next_wrap) :ALENextWrap
nnoremap <silent> <Plug>(ale_next) :ALENext
nnoremap <silent> <Plug>(ale_previous_wrap_warning) :ALEPrevious -wrap -warning
nnoremap <silent> <Plug>(ale_previous_warning) :ALEPrevious -warning
nnoremap <silent> <Plug>(ale_previous_wrap_error) :ALEPrevious -wrap -error
nnoremap <silent> <Plug>(ale_previous_error) :ALEPrevious -error
nnoremap <silent> <Plug>(ale_previous_wrap) :ALEPreviousWrap
nnoremap <silent> <Plug>(ale_previous) :ALEPrevious
map <C-N> :NERDTreeToggle
let &cpo=s:cpo_save
unlet s:cpo_save
set backspace=indent,eol,start
set backupdir=~/.cache/vim/backup//
set completeopt=menuone,noinsert,noselect,popuphidden
set completepopup=highlight:Pmenu,border:off
set directory=~/.cache/vim/swap//
set expandtab
set fileencodings=ucs-bom,utf-8,default,latin1
set nofixendofline
set helplang=en
set hidden
set hlsearch
set incsearch
set laststatus=2
set mouse=a
set runtimepath=~/.vim,~/.vim/bundle/vim-vroom,~/.vim/bundle/Vundle.vim,~/.vim/bundle/nerdtree,~/.vim/bundle/nerdtree-git-plugin,~/.vim/bundle/vim-tmux-navigator,~/.vim/bundle/vimspector,~/.vim/bundle/omnisharp-vim,~/.vim/bundle/vim-sharpenup,~/.vim/bundle/ale,~/.vim/bundle/asyncomplete.vim,~/.vim/bundle/gruvbox,~/.vim/bundle/lightline.vim,~/.vim/bundle/lightline-gruvbox.vim,~/.vim/bundle/lightline-ale,~/.vim/bundle/L9,~/.vim/bundle/Vundle.vim,~/.vim/bundle/ale,~/.vim/bundle/asyncomplete.vim,~/.vim/bundle/command-t,~/.vim/bundle/gruvbox,~/.vim/bundle/lightline-ale,~/.vim/bundle/lightline-gruvbox.vim,~/.vim/bundle/lightline.vim,~/.vim/bundle/nerdtree,~/.vim/bundle/nerdtree-git-plugin,~/.vim/bundle/omnisharp-vim,~/.vim/bundle/sparkup,~/.vim/bundle/vim-fugitive,~/.vim/bundle/vim-sharpenup,~/.vim/bundle/vim-tmux-navigator,~/.vim/bundle/vim-vroom,~/.vim/pack/vimspector/opt/vimspector,/usr/share/vim/vimfiles,/usr/share/vim/vim90,/usr/share/vim/vimfiles/after,~/.vim/bundle/nerdtree-git-plugin/after,~/.vim/after,~/.vim/bundle/vim-vroom/after,~/.vim/bundle/Vundle.vim/after,~/.vim/bundle/nerdtree/after,~/.vim/bundle/vim-tmux-navigator/after,~/.vim/bundle/omnisharp-vim/after,~/.vim/bundle/vim-sharpenup/after,~/.vim/bundle/ale/after,~/.vim/bundle/asyncomplete.vim/after,~/.vim/bundle/gruvbox/after,~/.vim/bundle/lightline.vim/after,~/.vim/bundle/lightline-gruvbox.vim/after,~/.vim/bundle/lightline-ale/after,~/.vim/bundle/vim-vroom/after,~/.vim/bundle/Vundle.vim/after,~/.vim/bundle/nerdtree/after,~/.vim/bundle/nerdtree-git-plugin/after,~/.vim/bundle/vim-tmux-navigator/after,~/.vim/bundle/vimspector/after,~/.vim/bundle/omnisharp-vim/after,~/.vim/bundle/vim-sharpenup/after,~/.vim/bundle/ale/after,~/.vim/bundle/asyncomplete.vim/after,~/.vim/bundle/gruvbox/after,~/.vim/bundle/lightline.vim/after,~/.vim/bundle/lightline-gruvbox.vim/after,~/.vim/bundle/lightline-ale/after
set shiftround
set shiftwidth=4
set noshowmode
set softtabstop=-1
set splitbelow
set splitright
set nostartofline
set suffixes=.bak,~,.o,.info,.swp,.aux,.bbl,.blg,.brf,.cb,.dvi,.idx,.ilg,.ind,.inx,.jpg,.log,.out,.png,.toc
set switchbuf=usetab
set tabline=%!lightline#tabline()
set termguicolors
set textwidth=80
set title
set undodir=~/.cache/vim/undo//
set updatetime=1000
let s:so_save = &g:so | let s:siso_save = &g:siso | setg so=0 siso=0 | setl so=-1 siso=-1
let v:this_session=expand("<sfile>:p")
silent only
silent tabonly
cd /run/media/scion/4b29c2d1-0afc-4483-9ae7-029e32ec28b8/repos/PerfRunner
if expand('%') == '' && !&modified && line('$') <= 1 && getline(1) == ''
  let s:wipebuf = bufnr('%')
endif
if &shortmess =~ 'A'
  set shortmess=aoOA
else
  set shortmess=aoO
endif
badd +53 ~/.vimrc
badd +0 PerfRunner/Program.cs
argglobal
%argdel
$argadd ~/.vimrc
set stal=2
tabnew +setlocal\ bufhidden=wipe
tabrewind
edit PerfRunner/Program.cs
let s:save_splitbelow = &splitbelow
let s:save_splitright = &splitright
set splitbelow splitright
wincmd _ | wincmd |
vsplit
1wincmd h
wincmd w
let &splitbelow = s:save_splitbelow
let &splitright = s:save_splitright
wincmd t
let s:save_winminheight = &winminheight
let s:save_winminwidth = &winminwidth
set winminheight=0
set winheight=1
set winminwidth=0
set winwidth=1
exe 'vert 1resize ' . ((&columns * 31 + 118) / 237)
exe 'vert 2resize ' . ((&columns * 205 + 118) / 237)
argglobal
enew
file NERD_tree_1
balt ~/.vimrc
let s:cpo_save=&cpo
set cpo&vim
nnoremap <buffer> <silent> <NL> :call nerdtree#ui_glue#invokeKeyMap("<C-j>")
nnoremap <buffer> <silent>  :call nerdtree#ui_glue#invokeKeyMap("<C-k>")
nnoremap <buffer> <silent>  :call nerdtree#ui_glue#invokeKeyMap("<CR>")
nnoremap <buffer> <silent> ? :call nerdtree#ui_glue#invokeKeyMap("?")
nnoremap <buffer> <silent> A :call nerdtree#ui_glue#invokeKeyMap("A")
nnoremap <buffer> <silent> B :call nerdtree#ui_glue#invokeKeyMap("B")
nnoremap <buffer> <silent> C :call nerdtree#ui_glue#invokeKeyMap("C")
nnoremap <buffer> <silent> CD :call nerdtree#ui_glue#invokeKeyMap("CD")
nnoremap <buffer> <silent> D :call nerdtree#ui_glue#invokeKeyMap("D")
nnoremap <buffer> <silent> F :call nerdtree#ui_glue#invokeKeyMap("F")
nnoremap <buffer> <silent> I :call nerdtree#ui_glue#invokeKeyMap("I")
nnoremap <buffer> <silent> J :call nerdtree#ui_glue#invokeKeyMap("J")
nnoremap <buffer> <silent> K :call nerdtree#ui_glue#invokeKeyMap("K")
nnoremap <buffer> <silent> O :call nerdtree#ui_glue#invokeKeyMap("O")
nnoremap <buffer> <silent> P :call nerdtree#ui_glue#invokeKeyMap("P")
nnoremap <buffer> <silent> R :call nerdtree#ui_glue#invokeKeyMap("R")
nnoremap <buffer> <silent> T :call nerdtree#ui_glue#invokeKeyMap("T")
nnoremap <buffer> <silent> U :call nerdtree#ui_glue#invokeKeyMap("U")
nnoremap <buffer> <silent> X :call nerdtree#ui_glue#invokeKeyMap("X")
nnoremap <buffer> <silent> [c :call nerdtree#ui_glue#invokeKeyMap("[c")
nnoremap <buffer> <silent> ]c :call nerdtree#ui_glue#invokeKeyMap("]c")
nnoremap <buffer> <silent> cd :call nerdtree#ui_glue#invokeKeyMap("cd")
nnoremap <buffer> <silent> e :call nerdtree#ui_glue#invokeKeyMap("e")
nnoremap <buffer> <silent> f :call nerdtree#ui_glue#invokeKeyMap("f")
nnoremap <buffer> <silent> go :call nerdtree#ui_glue#invokeKeyMap("go")
nnoremap <buffer> <silent> gi :call nerdtree#ui_glue#invokeKeyMap("gi")
nnoremap <buffer> <silent> gs :call nerdtree#ui_glue#invokeKeyMap("gs")
nnoremap <buffer> <silent> i :call nerdtree#ui_glue#invokeKeyMap("i")
nnoremap <buffer> <silent> m :call nerdtree#ui_glue#invokeKeyMap("m")
nnoremap <buffer> <silent> o :call nerdtree#ui_glue#invokeKeyMap("o")
nnoremap <buffer> <silent> p :call nerdtree#ui_glue#invokeKeyMap("p")
nnoremap <buffer> <silent> q :call nerdtree#ui_glue#invokeKeyMap("q")
nnoremap <buffer> <silent> r :call nerdtree#ui_glue#invokeKeyMap("r")
nnoremap <buffer> <silent> s :call nerdtree#ui_glue#invokeKeyMap("s")
nnoremap <buffer> <silent> t :call nerdtree#ui_glue#invokeKeyMap("t")
nnoremap <buffer> <silent> u :call nerdtree#ui_glue#invokeKeyMap("u")
nnoremap <buffer> <silent> x :call nerdtree#ui_glue#invokeKeyMap("x")
nnoremap <buffer> <silent> <MiddleMouse> :call nerdtree#ui_glue#invokeKeyMap("<MiddleMouse>")
nnoremap <buffer> <silent> <2-LeftMouse> :call nerdtree#ui_glue#invokeKeyMap("<2-LeftMouse>")
nnoremap <buffer> <silent> <C-K> :call nerdtree#ui_glue#invokeKeyMap("<C-k>")
nnoremap <buffer> <silent> <C-J> :call nerdtree#ui_glue#invokeKeyMap("<C-j>")
nnoremap <buffer> <silent> <LeftRelease> <LeftRelease>:call nerdtree#ui_glue#invokeKeyMap("<LeftRelease>")
let &cpo=s:cpo_save
unlet s:cpo_save
setlocal keymap=
setlocal noarabic
setlocal noautoindent
setlocal backupcopy=
setlocal balloonexpr=
setlocal nobinary
setlocal nobreakindent
setlocal breakindentopt=
setlocal bufhidden=hide
setlocal nobuflisted
setlocal buftype=nofile
setlocal nocindent
setlocal cinkeys=0{,0},0),0],:,0#,!^F,o,O,e
setlocal cinoptions=
setlocal cinscopedecls=public,protected,private
setlocal cinwords=if,else,while,do,for,switch
setlocal colorcolumn=
setlocal comments=s1:/*,mb:*,ex:*/,://,b:#,:%,:XCOMM,n:>,fb:-
setlocal commentstring=/*%s*/
setlocal complete=.,w,b,u,t,i
setlocal concealcursor=nvic
setlocal conceallevel=2
setlocal completefunc=
setlocal nocopyindent
setlocal cryptmethod=
setlocal nocursorbind
setlocal nocursorcolumn
setlocal cursorline
setlocal cursorlineopt=both
setlocal define=
setlocal dictionary=
setlocal nodiff
setlocal equalprg=
setlocal errorformat=
setlocal expandtab
if &filetype != 'nerdtree'
setlocal filetype=nerdtree
endif
setlocal fillchars=
setlocal nofixendofline
setlocal foldcolumn=0
setlocal nofoldenable
setlocal foldexpr=0
setlocal foldignore=#
setlocal foldlevel=0
setlocal foldmarker={{{,}}}
setlocal foldmethod=manual
setlocal foldminlines=1
setlocal foldnestmax=20
setlocal foldtext=foldtext()
setlocal formatexpr=
setlocal formatoptions=tcq
setlocal formatlistpat=^\\s*\\d\\+[\\]:.)}\\t\ ]\\s*
setlocal formatprg=
setlocal grepprg=
setlocal iminsert=0
setlocal imsearch=-1
setlocal include=
setlocal includeexpr=
setlocal indentexpr=
setlocal indentkeys=0{,0},0),0],:,0#,!^F,o,O,e
setlocal noinfercase
setlocal iskeyword=@,48-57,_,192-255
setlocal keywordprg=
setlocal nolinebreak
setlocal nolisp
setlocal lispoptions=
setlocal lispwords=
setlocal nolist
setlocal listchars=
setlocal makeencoding=
setlocal makeprg=
setlocal matchpairs=(:),{:},[:]
setlocal modeline
setlocal nomodifiable
setlocal nrformats=bin,octal,hex
setlocal nonumber
setlocal numberwidth=4
setlocal omnifunc=
setlocal path=
setlocal nopreserveindent
setlocal nopreviewwindow
setlocal quoteescape=\\
setlocal readonly
setlocal norelativenumber
setlocal norightleft
setlocal rightleftcmd=search
setlocal noscrollbind
setlocal scrolloff=-1
setlocal shiftwidth=4
setlocal noshortname
setlocal showbreak=
setlocal sidescrolloff=-1
set signcolumn=yes
setlocal signcolumn=yes
setlocal nosmartindent
setlocal nosmoothscroll
setlocal softtabstop=-1
setlocal nospell
setlocal spellcapcheck=[.?!]\\_[\\])'\"\	\ ]\\+
setlocal spellfile=
setlocal spelllang=en
setlocal spelloptions=
setlocal statusline=%{lightline#link()}%#LightlineLeft_inactive_0#%(\ %t\ %)%#LightlineLeft_inactive_0_1#%#LightlineMiddle_inactive#%=%#LightlineRight_inactive_2_3#%#LightlineRight_inactive_2#%(\ %{sharpenup#statusline#StatusText(0)}%{sharpenup#statusline#StatusText(1)}%{sharpenup#statusline#StatusText(2)}\ %)%#LightlineRight_inactive_1_2#%#LightlineRight_inactive_1#%(\ %3p%%\ %)%#LightlineRight_inactive_0_1#%#LightlineRight_inactive_0#%(\ %3l:%-2c\ %)
setlocal suffixesadd=
setlocal noswapfile
setlocal synmaxcol=3000
if &syntax != 'nerdtree'
setlocal syntax=nerdtree
endif
setlocal tabstop=8
setlocal tagcase=
setlocal tagfunc=
setlocal tags=
setlocal termwinkey=
setlocal termwinscroll=10000
setlocal termwinsize=
setlocal textwidth=80
setlocal thesaurus=
setlocal thesaurusfunc=
setlocal noundofile
setlocal undolevels=-123456
setlocal varsofttabstop=
setlocal vartabstop=
setlocal virtualedit=
setlocal wincolor=
setlocal nowinfixheight
setlocal winfixwidth
setlocal nowrap
setlocal wrapmargin=0
wincmd w
argglobal
balt ~/.vimrc
let s:cpo_save=&cpo
set cpo&vim
imap <buffer> <silent> <C-Bslash> <Plug>(omnisharp_signature_help)
inoremap <buffer> <Plug>(omnisharp_signature_help) :OmniSharpSignatureHelp
nmap <buffer> <silent>  osxr <Plug>(sharpenup_rename_in_csproj)
nmap <buffer> <silent>  osxa <Plug>(sharpenup_add_to_csproj)
nmap <buffer> <silent>  osrdnt <Plug>(omnisharp_debug_test_no_build)
nmap <buffer> <silent>  osrdt <Plug>(omnisharp_debug_test)
nmap <buffer> <silent>  osrant <Plug>(omnisharp_run_tests_in_file_no_build)
nmap <buffer> <silent>  osrat <Plug>(omnisharp_run_tests_in_file)
nmap <buffer> <silent>  osrnt <Plug>(omnisharp_run_test_no_build)
nmap <buffer> <silent>  osrt <Plug>(omnisharp_run_test)
nmap <buffer> <silent>  ossp <Plug>(omnisharp_stop_server)
nmap <buffer> <silent>  osst <Plug>(omnisharp_start_server)
nmap <buffer> <silent>  osre <Plug>(omnisharp_restart_server)
nmap <buffer> <silent>  os= <Plug>(omnisharp_code_format)
nmap <buffer> <silent>  osnm <Plug>(omnisharp_rename)
xmap <buffer> <silent>  os. <Plug>(omnisharp_code_action_repeat)
nmap <buffer> <silent>  os. <Plug>(omnisharp_code_action_repeat)
xmap <buffer> <silent>  osca <Plug>(omnisharp_code_actions)
nmap <buffer> <silent>  osca <Plug>(omnisharp_code_actions)
nmap <buffer> <silent>  oshi <Plug>(omnisharp_highlight_types)
nmap <buffer> <silent>  osgcc <Plug>(omnisharp_global_code_check)
nmap <buffer> <silent>  osfx <Plug>(omnisharp_fix_usings)
nmap <buffer> <silent>  osft <Plug>(omnisharp_find_type)
nmap <buffer> <silent>  osfs <Plug>(omnisharp_find_symbol)
nmap <buffer> <silent>  osd <Plug>(omnisharp_documentation)
nmap <buffer> <silent>  ost <Plug>(omnisharp_type_lookup)
nmap <buffer> <silent>  ospi <Plug>(omnisharp_preview_implementation)
nmap <buffer> <silent>  ospd <Plug>(omnisharp_preview_definition)
nmap <buffer> <silent>  osfi <Plug>(omnisharp_find_implementations)
nmap <buffer> <silent>  osfu <Plug>(omnisharp_find_usages)
nmap <buffer> <silent>  osgt <Plug>(omnisharp_go_to_type_definition)
nmap <buffer> <silent> [[ <Plug>(omnisharp_navigate_up)
nmap <buffer> <silent> ]] <Plug>(omnisharp_navigate_down)
nmap <buffer> <silent> gd <Plug>(omnisharp_go_to_definition)
nnoremap <buffer> <Plug>(sharpenup_rename_in_csproj) :call sharpenup#legacycsproj#RenameInProjectPopulate()
nnoremap <buffer> <Plug>(sharpenup_add_to_csproj) :call sharpenup#legacycsproj#AddToProject()
nnoremap <buffer> <Plug>(omnisharp_highlight_types) :OmniSharpHighlight
nnoremap <buffer> <Plug>(omnisharp_create_debug_config) :OmniSharpCreateDebugConfig
nnoremap <buffer> <Plug>(omnisharp_debug_project) :OmniSharpDebugProject
nnoremap <buffer> <Plug>(omnisharp_type_lookup) :OmniSharpTypeLookup
nnoremap <buffer> <Plug>(omnisharp_stop_server) :OmniSharpStopServer
nnoremap <buffer> <Plug>(omnisharp_stop_all_servers) :OmniSharpStopAllServers
nnoremap <buffer> <Plug>(omnisharp_start_server) :OmniSharpStartServer
nnoremap <buffer> <Plug>(omnisharp_signature_help) :OmniSharpSignatureHelp
nnoremap <buffer> <Plug>(omnisharp_run_tests_in_file_no_build) :OmniSharpRunTestsInFile!
nnoremap <buffer> <Plug>(omnisharp_run_tests_in_file) :OmniSharpRunTestsInFile
nnoremap <buffer> <Plug>(omnisharp_debug_test_no_build) :OmniSharpDebugTest!
nnoremap <buffer> <Plug>(omnisharp_debug_test) :OmniSharpDebugTest
nnoremap <buffer> <Plug>(omnisharp_run_test_no_build) :OmniSharpRunTest!
nnoremap <buffer> <Plug>(omnisharp_run_test) :OmniSharpRunTest
nnoremap <buffer> <Plug>(omnisharp_restart_all_servers) :OmniSharpRestartAllServers
nnoremap <buffer> <Plug>(omnisharp_restart_server) :OmniSharpRestartServer
nnoremap <buffer> <Plug>(omnisharp_rename) :OmniSharpRename
nnoremap <buffer> <Plug>(omnisharp_reload_project) :OmniSharpReloadProject
nnoremap <buffer> <Plug>(omnisharp_preview_implementation) :OmniSharpPreviewImplementation
nnoremap <buffer> <Plug>(omnisharp_preview_definition) :OmniSharpPreviewDefinition
nnoremap <buffer> <Plug>(omnisharp_navigate_down) :OmniSharpNavigateDown
nnoremap <buffer> <Plug>(omnisharp_navigate_up) :OmniSharpNavigateUp
nnoremap <buffer> <Plug>(omnisharp_highlight) :OmniSharpHighlight
nnoremap <buffer> <Plug>(omnisharp_go_to_definition) :OmniSharpGotoDefinition
nnoremap <buffer> <Plug>(omnisharp_go_to_type_definition) :OmniSharpGotoTypeDefinition
nnoremap <buffer> <Plug>(omnisharp_global_code_check) :OmniSharpGlobalCodeCheck
xnoremap <buffer> <Plug>(omnisharp_code_action_repeat) :call OmniSharp#actions#codeactions#Repeat('visual')
nnoremap <buffer> <Plug>(omnisharp_code_action_repeat) :OmniSharpRepeatCodeAction
xnoremap <buffer> <Plug>(omnisharp_code_actions) :call OmniSharp#actions#codeactions#Get('visual')
nnoremap <buffer> <Plug>(omnisharp_code_actions) :OmniSharpGetCodeActions
nnoremap <buffer> <Plug>(omnisharp_fix_usings) :OmniSharpFixUsings
nnoremap <buffer> <Plug>(omnisharp_find_usages) :OmniSharpFindUsages
nnoremap <buffer> <Plug>(omnisharp_find_type) :OmniSharpFindType
nnoremap <buffer> <Plug>(omnisharp_find_symbol) :OmniSharpFindSymbol
nnoremap <buffer> <Plug>(omnisharp_find_members) :OmniSharpFindMembers
nnoremap <buffer> <Plug>(omnisharp_find_implementations) :OmniSharpFindImplementations
nnoremap <buffer> <Plug>(omnisharp_documentation) :OmniSharpDocumentation
nnoremap <buffer> <Plug>(omnisharp_code_format) :OmniSharpCodeFormat
imap <buffer> <silent>  <Plug>(omnisharp_signature_help)
let &cpo=s:cpo_save
unlet s:cpo_save
setlocal keymap=
setlocal noarabic
setlocal noautoindent
setlocal backupcopy=
setlocal balloonexpr=
setlocal nobinary
setlocal nobreakindent
setlocal breakindentopt=
setlocal bufhidden=
setlocal buflisted
setlocal buftype=
setlocal nocindent
setlocal cinkeys=0{,0},0),0],:,0#,!^F,o,O,e
setlocal cinoptions=
setlocal cinscopedecls=public,protected,private
setlocal cinwords=if,else,while,do,for,switch
setlocal colorcolumn=
setlocal comments=sO:*\ -,mO:*\ \ ,exO:*/,s1:/*,mb:*,ex:*/,:///,://
setlocal commentstring=/*%s*/
setlocal complete=.,w,b,u,t,i
setlocal concealcursor=
setlocal conceallevel=0
setlocal completefunc=
setlocal nocopyindent
setlocal cryptmethod=
setlocal nocursorbind
setlocal nocursorcolumn
setlocal nocursorline
setlocal cursorlineopt=both
setlocal define=
setlocal dictionary=
setlocal nodiff
setlocal equalprg=
setlocal errorformat=
setlocal expandtab
if &filetype != 'cs'
setlocal filetype=cs
endif
setlocal fillchars=
setlocal nofixendofline
setlocal foldcolumn=0
setlocal foldenable
setlocal foldexpr=0
setlocal foldignore=#
setlocal foldlevel=0
setlocal foldmarker={{{,}}}
setlocal foldmethod=manual
setlocal foldminlines=1
setlocal foldnestmax=20
setlocal foldtext=foldtext()
setlocal formatexpr=
setlocal formatoptions=croql
setlocal formatlistpat=^\\s*\\d\\+[\\]:.)}\\t\ ]\\s*
setlocal formatprg=
setlocal grepprg=
setlocal iminsert=0
setlocal imsearch=-1
setlocal include=
setlocal includeexpr=
setlocal indentexpr=GetCSIndent(v:lnum)
setlocal indentkeys=0{,0},0),0],:,0#,!^F,o,O,e
setlocal noinfercase
setlocal iskeyword=@,48-57,_,192-255
setlocal keywordprg=
setlocal nolinebreak
setlocal nolisp
setlocal lispoptions=
setlocal lispwords=
setlocal nolist
setlocal listchars=
setlocal makeencoding=
setlocal makeprg=
setlocal matchpairs=(:),{:},[:]
setlocal modeline
setlocal modifiable
setlocal nrformats=bin,octal,hex
setlocal nonumber
setlocal numberwidth=4
setlocal omnifunc=OmniSharp#Complete
setlocal path=
setlocal nopreserveindent
setlocal nopreviewwindow
setlocal quoteescape=\\
setlocal noreadonly
setlocal norelativenumber
setlocal norightleft
setlocal rightleftcmd=search
setlocal noscrollbind
setlocal scrolloff=-1
setlocal shiftwidth=4
setlocal noshortname
setlocal showbreak=
setlocal sidescrolloff=-1
set signcolumn=yes
setlocal signcolumn=yes
setlocal nosmartindent
setlocal nosmoothscroll
setlocal softtabstop=-1
setlocal nospell
setlocal spellcapcheck=[.?!]\\_[\\])'\"\	\ ]\\+
setlocal spellfile=
setlocal spelllang=en
setlocal spelloptions=
setlocal statusline=%{lightline#link()}%#LightlineLeft_active_0#%(\ %{lightline#mode()}\ %)%{(&paste)?\"|\":\"\"}%(\ %{&paste?\"PASTE\":\"\"}\ %)%#LightlineLeft_active_0_1#%#LightlineLeft_active_1#%(\ %R\ %)%{(&readonly)&&(1||(&modified||!&modifiable))?\"|\":\"\"}%(\ %t\ %)%{(&modified||!&modifiable)?\"|\":\"\"}%(\ %M\ %)%#LightlineLeft_active_1_2#%#LightlineMiddle_active#%=%#LightlineRight_active_3_4#%#LightlineRight_active_3#%(\ %{&ff}\ %)%{1||1||1?\"|\":\"\"}%(\ %{&fenc!=#\"\"?&fenc:&enc}\ %)%{1||1?\"|\":\"\"}%(\ %{&ft!=#\"\"?&ft:\"no\ ft\"}\ %)%{1?\"|\":\"\"}%(\ %{sharpenup#statusline#StatusText(0)}%{sharpenup#statusline#StatusText(1)}%{sharpenup#statusline#StatusText(2)}\ %)%#LightlineRight_active_2_3#%#LightlineRight_active_2#%(\ %3p%%\ %)%#LightlineRight_active_1_2#%#LightlineRight_active_1#%(\ %3l:%-2c\ %)%#LightlineRight_active_right_1#%#LightlineRight_active_right#%(\ ÔÑ©\ 29\ %)%#LightlineRight_active_warning_right#%#LightlineRight_active_warning#%(\ ÔÅ±\ 16\ %)
setlocal suffixesadd=
setlocal swapfile
setlocal synmaxcol=3000
if &syntax != 'cs'
setlocal syntax=cs
endif
setlocal tabstop=8
setlocal tagcase=
setlocal tagfunc=
setlocal tags=
setlocal termwinkey=
setlocal termwinscroll=10000
setlocal termwinsize=
setlocal textwidth=80
setlocal thesaurus=
setlocal thesaurusfunc=
setlocal noundofile
setlocal undolevels=-123456
setlocal varsofttabstop=
setlocal vartabstop=
setlocal virtualedit=
setlocal wincolor=
setlocal nowinfixheight
setlocal nowinfixwidth
setlocal wrap
setlocal wrapmargin=0
silent! normal! zE
let &fdl = &fdl
let s:l = 3 - ((2 * winheight(0) + 26) / 53)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 3
normal! 0
wincmd w
exe 'vert 1resize ' . ((&columns * 31 + 118) / 237)
exe 'vert 2resize ' . ((&columns * 205 + 118) / 237)
tabnext
edit ~/.vimrc
argglobal
balt PerfRunner/Program.cs
vnoremap <buffer> <silent> [" :exe "normal! gv"|call search('\%(^\s*".*\n\)\%(^\s*"\)\@!', "bW")
nnoremap <buffer> <silent> [" :call search('\%(^\s*".*\n\)\%(^\s*"\)\@!', "bW")
vnoremap <buffer> <silent> [] m':exe "normal! gv"|call search('^\s*end\(f\%[unction]\|\(export\s\+\)\?def\)\>', "bW")
nnoremap <buffer> <silent> [] m':call search('^\s*end\(f\%[unction]\|\(export\s\+\)\?def\)\>', "bW")
vnoremap <buffer> <silent> [[ m':exe "normal! gv"|call search('^\s*\(fu\%[nction]\|\(export\s\+\)\?def\)\>', "bW")
nnoremap <buffer> <silent> [[ m':call search('^\s*\(fu\%[nction]\|\(export\s\+\)\?def\)\>', "bW")
vnoremap <buffer> <silent> ]" :exe "normal! gv"|call search('^\(\s*".*\n\)\@<!\(\s*"\)', "W")
nnoremap <buffer> <silent> ]" :call search('^\(\s*".*\n\)\@<!\(\s*"\)', "W")
vnoremap <buffer> <silent> ][ m':exe "normal! gv"|call search('^\s*end\(f\%[unction]\|\(export\s\+\)\?def\)\>', "W")
nnoremap <buffer> <silent> ][ m':call search('^\s*end\(f\%[unction]\|\(export\s\+\)\?def\)\>', "W")
vnoremap <buffer> <silent> ]] m':exe "normal! gv"|call search('^\s*\(fu\%[nction]\|\(export\s\+\)\?def\)\>', "W")
nnoremap <buffer> <silent> ]] m':call search('^\s*\(fu\%[nction]\|\(export\s\+\)\?def\)\>', "W")
setlocal keymap=
setlocal noarabic
setlocal noautoindent
setlocal backupcopy=
setlocal balloonexpr=
setlocal nobinary
setlocal nobreakindent
setlocal breakindentopt=
setlocal bufhidden=
setlocal buflisted
setlocal buftype=
setlocal nocindent
setlocal cinkeys=0{,0},0),0],:,0#,!^F,o,O,e
setlocal cinoptions=
setlocal cinscopedecls=public,protected,private
setlocal cinwords=if,else,while,do,for,switch
setlocal colorcolumn=
setlocal comments=sO:#\ -,mO:#\ \ ,eO:##,:#,sO:\"\ -,mO:\"\ \ ,eO:\"\",:\"
setlocal commentstring=\"%s
setlocal complete=.,w,b,u,t,i
setlocal concealcursor=
setlocal conceallevel=0
setlocal completefunc=
setlocal nocopyindent
setlocal cryptmethod=
setlocal nocursorbind
setlocal nocursorcolumn
setlocal nocursorline
setlocal cursorlineopt=both
setlocal define=\\v^\\s*export\\s*(def|const|var|final)
setlocal dictionary=
setlocal nodiff
setlocal equalprg=
setlocal errorformat=
setlocal expandtab
if &filetype != 'vim'
setlocal filetype=vim
endif
setlocal fillchars=
setlocal nofixendofline
setlocal foldcolumn=0
setlocal foldenable
setlocal foldexpr=0
setlocal foldignore=#
setlocal foldlevel=0
setlocal foldmarker={{{,}}}
setlocal foldmethod=manual
setlocal foldminlines=1
setlocal foldnestmax=20
setlocal foldtext=foldtext()
setlocal formatexpr=
setlocal formatoptions=croql
setlocal formatlistpat=^\\s*\\d\\+[\\]:.)}\\t\ ]\\s*
setlocal formatprg=
setlocal grepprg=
setlocal iminsert=0
setlocal imsearch=-1
setlocal include=\\v^\\s*import\\s*(autoload)?
setlocal includeexpr=
setlocal indentexpr=vimindent.Expr()
setlocal indentkeys=0{,0},0),0],!^F,o,O,e,=endif,=enddef,=endfu,=endfor,=endwh,=endtry,=endclass,=endinterface,=endenum,=},=else,=cat,=finall,=END,0\\,0=\"\\\ ,0=#\\\ 
setlocal noinfercase
setlocal iskeyword=@,48-57,_,192-255,#
setlocal keywordprg=:help
setlocal nolinebreak
setlocal nolisp
setlocal lispoptions=
setlocal lispwords=
setlocal nolist
setlocal listchars=
setlocal makeencoding=
setlocal makeprg=
setlocal matchpairs=(:),{:},[:]
setlocal modeline
setlocal modifiable
setlocal nrformats=bin,octal,hex
setlocal nonumber
setlocal numberwidth=4
setlocal omnifunc=
setlocal path=
setlocal nopreserveindent
setlocal nopreviewwindow
setlocal quoteescape=\\
setlocal noreadonly
setlocal norelativenumber
setlocal norightleft
setlocal rightleftcmd=search
setlocal noscrollbind
setlocal scrolloff=-1
setlocal shiftwidth=4
setlocal noshortname
setlocal showbreak=
setlocal sidescrolloff=-1
set signcolumn=yes
setlocal signcolumn=yes
setlocal nosmartindent
setlocal nosmoothscroll
setlocal softtabstop=-1
setlocal nospell
setlocal spellcapcheck=[.?!]\\_[\\])'\"\	\ ]\\+
setlocal spellfile=
setlocal spelllang=en
setlocal spelloptions=
setlocal statusline=%{lightline#link()}%#LightlineLeft_active_0#%(\ %{lightline#mode()}\ %)%{(&paste)?\"|\":\"\"}%(\ %{&paste?\"PASTE\":\"\"}\ %)%#LightlineLeft_active_0_1#%#LightlineLeft_active_1#%(\ %R\ %)%{(&readonly)&&(1||(&modified||!&modifiable))?\"|\":\"\"}%(\ %t\ %)%{(&modified||!&modifiable)?\"|\":\"\"}%(\ %M\ %)%#LightlineLeft_active_1_2#%#LightlineMiddle_active#%=%#LightlineRight_active_3_4#%#LightlineRight_active_3#%(\ %{&ff}\ %)%{1||1||1?\"|\":\"\"}%(\ %{&fenc!=#\"\"?&fenc:&enc}\ %)%{1||1?\"|\":\"\"}%(\ %{&ft!=#\"\"?&ft:\"no\ ft\"}\ %)%{1?\"|\":\"\"}%(\ %{sharpenup#statusline#StatusText(0)}%{sharpenup#statusline#StatusText(1)}%{sharpenup#statusline#StatusText(2)}\ %)%#LightlineRight_active_2_3#%#LightlineRight_active_2#%(\ %3p%%\ %)%#LightlineRight_active_1_2#%#LightlineRight_active_1#%(\ %3l:%-2c\ %)%#LightlineRight_active_right_1#%#LightlineRight_active_right#%(\ ÔÄå\ \ %)
setlocal suffixesadd=
setlocal swapfile
setlocal synmaxcol=3000
if &syntax != 'vim'
setlocal syntax=vim
endif
setlocal tabstop=8
setlocal tagcase=
setlocal tagfunc=
setlocal tags=
setlocal termwinkey=
setlocal termwinscroll=10000
setlocal termwinsize=
setlocal textwidth=80
setlocal thesaurus=
setlocal thesaurusfunc=
setlocal noundofile
setlocal undolevels=-123456
setlocal varsofttabstop=
setlocal vartabstop=
setlocal virtualedit=
setlocal wincolor=
setlocal nowinfixheight
setlocal nowinfixwidth
setlocal wrap
setlocal wrapmargin=0
silent! normal! zE
let &fdl = &fdl
let s:l = 53 - ((44 * winheight(0) + 26) / 52)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 53
normal! 0
tabnext 2
set stal=1
if exists('s:wipebuf') && len(win_findbuf(s:wipebuf)) == 0
  silent exe 'bwipe ' . s:wipebuf
endif
unlet! s:wipebuf
set winheight=1 winwidth=20
set shortmess=filnxtToOS
let &winminheight = s:save_winminheight
let &winminwidth = s:save_winminwidth
let s:sx = expand("<sfile>:p:r")."x.vim"
if filereadable(s:sx)
  exe "source " . fnameescape(s:sx)
endif
let &g:so = s:so_save | let &g:siso = s:siso_save
doautoall SessionLoadPost
unlet SessionLoad
" vim: set ft=vim :
