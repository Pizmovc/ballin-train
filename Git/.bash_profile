# Enable tab completion
source ~/git-completion.bash

# Change command prompt
source ~/git-prompt.sh
export GIT_PS1_SHOWDIRTYSTATE=1
# '\u' adds the name of the current user to the prompt
# '\$(__git_ps1)' adds git-related stuff
# '\W' adds the name of the current directory
export PS1="\e[0;33m[\d]\e[m \e[0;35m\$(__git_ps1)\e[m \e[0;32m\w \e[m\n\e[0;31mInput: \e[m"
							 
